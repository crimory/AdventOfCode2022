mod steps;

use crate::day16_2::{find, input};

pub fn enrich_moves_with_next_step(
    moves: Vec<find::StepsAndAccompanyingValues>,
    map: &input::Map,
) -> Vec<find::StepsAndAccompanyingValues> {
    moves.iter()
        .flat_map(|m| {
            let next_steps = steps::get_next_steps(&m.steps[0], &m.valves_left_to_open, map);
            next_steps
                .iter()
                .map(|s| {
                    let pressure_being_released = m.pressure_being_released;
                    let mut valves_left_to_open = m.valves_left_to_open.clone();
                    let pressure_being_released = adjust_steps_and_accompanying_values(map, &pressure_being_released, &mut valves_left_to_open, s.last().unwrap());
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

fn combine_steps(
    your_steps: &Vec<Vec<find::StepAction>>,
    elephant_steps: &Vec<Vec<find::StepAction>>,
) -> Vec<Vec<Vec<find::StepAction>>> {
    let mut output = vec![];
    for yours in your_steps {
        for elephants in elephant_steps {
            let a = output.contains(&vec![yours.to_owned(), elephants.to_owned()]);
            let b = output.contains(&vec![elephants.to_owned(), yours.to_owned()]);
            if a || b {
                continue;
            }
            output.push(vec![yours.to_owned(), elephants.to_owned()]);
        }
    }
    output
}

fn adjust_steps_and_accompanying_values(
    map: &input::Map,
    pressure_being_released: &u32,
    valves_left_to_open: &mut Vec<u32>,
    potentially_opened_index: &find::StepAction,
) -> u32 {
    if let find::StepAction::OpenValve(opened) = potentially_opened_index {
        if !valves_left_to_open.contains(opened) {
            return *pressure_being_released;
        }
        valves_left_to_open.retain(|v| v != opened);
        return *pressure_being_released + map.get_valve_by_index(opened).flow_rate;
    }
    *pressure_being_released
}

pub fn enrich_moves_with_next_step_for_two(
    moves: Vec<find::StepsAndAccompanyingValues>,
    map: &input::Map,
) -> Vec<find::StepsAndAccompanyingValues> {
    moves.iter()
        .flat_map(|m| {
            let next_steps_yours = steps::get_next_steps(&m.steps[0], &m.valves_left_to_open, map);
            let next_steps_elephants = steps::get_next_steps(&m.steps[1], &m.valves_left_to_open, map);
            let next_steps = combine_steps(&next_steps_yours, &next_steps_elephants);
            next_steps
                .iter()
                .map(|s| {
                    let pressure_being_released = m.pressure_being_released;
                    let mut valves_left_to_open = m.valves_left_to_open.clone();
                    let pressure_being_released = adjust_steps_and_accompanying_values(map, &pressure_being_released, &mut valves_left_to_open, s[0].last().unwrap());
                    let pressure_being_released = adjust_steps_and_accompanying_values(map, &pressure_being_released, &mut valves_left_to_open, s[1].last().unwrap());
                    find::StepsAndAccompanyingValues {
                        steps: vec![s[0].to_vec(), s[1].to_vec()],
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
        .filter(|m| !m.steps.iter().any(steps::is_moving_back_and_forth))
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
