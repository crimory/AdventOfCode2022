mod paths;

use crate::day16_2::input;

#[derive(Clone, Debug, PartialEq)]
pub enum StepAction {
    OpenValve(u32),
    Move(u32),
    Finished,
}

#[derive(Clone)]
pub struct StepsAndAccompanyingValues {
    steps: Vec<Vec<StepAction>>,
    valves_left_to_open: Vec<u32>,
    pressure_released: u32,
    pressure_being_released: u32,
}
impl StepsAndAccompanyingValues {
    fn new(initial_step: u32, number_of_actors: usize, map: &input::Map) -> StepsAndAccompanyingValues {
        let valves_left_to_open = map.valves.iter()
            .filter(|v| v.flow_rate > 0)
            .map(|v| v.index)
            .collect();
        let steps = vec![vec![StepAction::Move(initial_step)]; number_of_actors];
        StepsAndAccompanyingValues {
            steps,
            valves_left_to_open,
            pressure_released: 0,
            pressure_being_released: 0,
        }
    }
}

const MAX_ITEMS: usize = 500;
pub fn get_max_pressure_within_minutes(minutes: u32, map: &input::Map) -> u32 {
    let mut all_moves = vec![StepsAndAccompanyingValues::new(0, 1, map); 1];
    for _ in 1..=minutes {
        all_moves = paths::enrich_moves_with_next_step(all_moves, map);
        all_moves = paths::filter_out_back_and_forth_moves(all_moves);
        all_moves = paths::get_maximum_number_of_moves(all_moves, MAX_ITEMS);
    }
    all_moves.sort_by(|a, b| b.pressure_released.cmp(&a.pressure_released));
    all_moves.first().unwrap().pressure_released
}

pub fn get_max_pressure_for_two(minutes: u32, map: &input::Map) -> u32 {
    let mut all_moves = vec![StepsAndAccompanyingValues::new(0, 2, map); 1];
    for _ in 1..=minutes {
        all_moves = paths::enrich_moves_with_next_step(all_moves, map);
        all_moves = paths::filter_out_back_and_forth_moves(all_moves);
        all_moves = paths::get_maximum_number_of_moves(all_moves, MAX_ITEMS);
    }
    all_moves.sort_by(|a, b| b.pressure_released.cmp(&a.pressure_released));
    all_moves.first().unwrap().pressure_released
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn get_max_pressure_within_minutes_test() {
        let map = input::read_input(input::tests::INPUT);
        let result = get_max_pressure_within_minutes(30, &map);
        assert_eq!(1651, result);
    }

    #[test]
    fn get_max_pressure_for_two_test() {
        let map = input::read_input(input::tests::INPUT);
        let result = get_max_pressure_for_two(26, &map);
        assert_eq!(1707, result);
    }
}