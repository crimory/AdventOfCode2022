use std::collections::HashMap;

mod input;
mod map;
mod rocks;

fn get_rocks_sideways_movement(air: &input::AirJetBlowingDirection) -> rocks::ShapeMove {
    rocks::ShapeMove::Sideways(match air {
        input::AirJetBlowingDirection::Left => rocks::Side::Left,
        input::AirJetBlowingDirection::Right => rocks::Side::Right,
    })
}

fn inner_progress_single_rock(
    map: &mut map::Map,
    rock_source: &mut rocks::RockSource,
    air_jets: &mut input::AirJets,
) -> u64 {
    let new_rock_position = map.get_new_shape_position();
    let mut rock = rocks::ShapeState::Falling(rock_source.next().to_shape(&new_rock_position));
    let mut air_jets_advance = 0;
    while let rocks::ShapeState::Falling(_) = &rock {
        rock.move_shape(get_rocks_sideways_movement(air_jets.next()), &map);
        rock.move_shape(rocks::ShapeMove::Down, &map);
        air_jets_advance += 1;
    }
    if let rocks::ShapeState::Settled(settled_shape) = rock {
        map.add_settled_shape(settled_shape);
    }
    air_jets_advance
}

#[derive(Eq, Hash, PartialEq)]
struct MapSnapshotInput {
    current_height: Vec<u64>,
    air_jet_index: usize,
    rock_source_index: usize,
}

struct MapSnapshotOutput {
    map: map::Map,
    height_gained: u64,
    air_jets_advance: u64,
    next_state: MapSnapshotInput,
}

pub fn how_tall_tower(rounds: u64, input: &str) -> u64 {
    let mut rock_map_snapshot_repo: HashMap<MapSnapshotInput, MapSnapshotOutput> = HashMap::new();
    let mut map = map::Map::new();
    let mut air_jets = input::read_input(input);
    let mut rock_source = rocks::RockSource::new();

    let mut height_counter = 0;
    let mut index = 0;
    while index < rounds {
        let map_snapshot_input = MapSnapshotInput {
            current_height: map.get_current_heights(),
            air_jet_index: air_jets.current_index(),
            rock_source_index: rock_source.current_index(),
        };
        match rock_map_snapshot_repo.get(&map_snapshot_input) {
            None => {
                let air_jets_advance = inner_progress_single_rock(&mut map, &mut rock_source, &mut air_jets);
                let height_gained = map.retain_top_of_the_map();
                height_counter += height_gained;
                index += 1;

                rock_map_snapshot_repo.insert(
                    map_snapshot_input,
                    MapSnapshotOutput {
                        map: map.clone(),
                        height_gained,
                        air_jets_advance,
                        next_state: MapSnapshotInput {
                            current_height: map.get_current_heights(),
                            air_jet_index: air_jets.current_index(),
                            rock_source_index: rock_source.current_index(),
                        },
                    },
                );
            },
            Some(initial_snapshot) => {
                let index_before_loop = index;
                index += 1;
                rock_source.next();
                let mut height_gained = initial_snapshot.height_gained;
                let mut air_jets_advance = initial_snapshot.air_jets_advance;
                let mut map_ref = &initial_snapshot.map;
                let mut next_step_ref = &initial_snapshot.next_state;
                while let Some(inner_snapshot) = rock_map_snapshot_repo.get(next_step_ref) {
                    height_gained += inner_snapshot.height_gained;
                    air_jets_advance += inner_snapshot.air_jets_advance;
                    map_ref = &inner_snapshot.map;
                    next_step_ref = &inner_snapshot.next_state;
                    index += 1;
                    rock_source.next();
                    if index == rounds {
                        break;
                    }
                    if next_step_ref == &map_snapshot_input {
                        let index_change = index - index_before_loop;
                        let remaining_rounds = rounds - index;
                        let cycles = remaining_rounds / index_change;
                        index += cycles * index_change;
                        rock_source.advance_index(cycles * index_change);
                        air_jets_advance += cycles * air_jets_advance;
                        height_counter += cycles * height_gained;
                        break;
                    }
                }
                height_counter += height_gained;
                air_jets.advance_index(air_jets_advance);
                map = map_ref.clone();
            }
        }
    }

    map.get_current_height() + height_counter
}

#[cfg(test)]
mod tests {
    use super::*;

    const INPUT: &str = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";

    #[test]
    fn how_tall_min() {
        let result = how_tall_tower(10, &INPUT);
        assert_eq!(result, 17);
    }

    #[test]
    fn how_tall() {
        let result = how_tall_tower(2022, &INPUT);
        assert_eq!(result, 3068);
    }

    #[test]
    fn how_tall_bigger_example() {
        let result = how_tall_tower(1_000_000_000_000, &INPUT);
        assert_eq!(result, 1_514_285_714_288);
    }
}
