mod input;
mod map;
mod rocks;

fn get_rocks_sideways_movement(air: &input::AirJetBlowingDirection) -> rocks::ShapeMove {
    rocks::ShapeMove::Sideways(match air {
        input::AirJetBlowingDirection::Left => rocks::Side::Left,
        input::AirJetBlowingDirection::Right => rocks::Side::Right,
    })
}

pub fn how_tall_tower(rounds: u64, input: &str) -> u64 {
    let mut air_jets = input::read_input(input);
    let mut map = map::Map::new();
    let mut rock_source = rocks::RockSource::new();
    for _ in 0..rounds {
        let new_rock_position = map.get_new_shape_position();
        let mut rock = rocks::ShapeState::Falling(rock_source.next().to_shape(&new_rock_position));
        while let rocks::ShapeState::Falling(_) = &rock {
            rock.move_shape(get_rocks_sideways_movement(air_jets.next()), &map);
            rock.move_shape(rocks::ShapeMove::Down, &map);
        }
        if let rocks::ShapeState::Settled(settled_shape) = rock {
            map.add_settled_shape(settled_shape);
        }
    }
    map.get_current_height()
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
