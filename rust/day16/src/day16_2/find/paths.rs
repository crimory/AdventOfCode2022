mod steps;

use crate::day16_2::{find, input};

pub fn enrich_moves_with_next_step(
    moves: Vec<find::StepsAndAccompanyingValues>,
    map: &input::Map,
) -> Vec<find::StepsAndAccompanyingValues> {
    moves.iter()
        .flat_map(|m| {
            let mut next_steps = steps::get_next_steps(&m.steps[0], &m.valves_left_to_open, &map);
            next_steps
                .iter()
                .map(|s| {
                    let mut pressure_being_released = m.pressure_being_released;
                    let mut valves_left_to_open = m.valves_left_to_open.clone();
                    if let find::StepAction::OpenValve(opened) = s.last().unwrap() {
                        valves_left_to_open.retain(|v| v != opened);
                        pressure_being_released += map.get_valve_by_index(opened).flow_rate;
                    }
                    find::StepsAndAccompanyingValues {
                        steps: vec![s.to_vec()],
                        valves_left_to_open,
                        pressure_being_released,
                        pressure_released: m.pressure_released + m.pressure_being_released,
                    }
                })
                .collect::<Vec<find::StepsAndAccompanyingValues>>()
        })
        .collect::<Vec<find::StepsAndAccompanyingValues>>()
}

pub fn filter_out_back_and_forth_moves(
    moves: Vec<find::StepsAndAccompanyingValues>,
) -> Vec<find::StepsAndAccompanyingValues> {
    moves.iter()
        .filter(|m| {
            if steps::is_moving_back_and_forth(&m.steps[0]) {
                return false;
            }
            true
        })
        .map(|m| m.to_owned())
        .collect::<Vec<find::StepsAndAccompanyingValues>>()
}

pub fn get_maximum_number_of_moves(
    moves: Vec<find::StepsAndAccompanyingValues>,
    max_number_of_items: usize,
) -> Vec<find::StepsAndAccompanyingValues> {
    if moves.len() > max_number_of_items {
        let mut sorted_moves = moves.to_vec();
        sorted_moves.sort_by(|a, b| b.pressure_released.cmp(&a.pressure_released));
        return sorted_moves[0..max_number_of_items].to_vec();
    }
    moves.to_vec()
}
