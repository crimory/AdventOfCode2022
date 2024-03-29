use super::input;
use std::collections::HashMap;

#[derive(Clone, Debug, PartialEq)]
enum StepAction {
    OpenValve(String),
    Move(String),
    Finished,
}

fn paths_to_valve_next_steps(
    paths: &Vec<Vec<String>>,
    map: &HashMap<String, input::Valve>,
) -> Vec<Vec<String>> {
    let mut output_paths = Vec::new();
    for path in paths {
        let current_valve = map.get(path.last().unwrap()).unwrap();
        let previous_valve = if path.len() > 1 {
            path.get(path.len() - 2).unwrap()
        } else {
            path.last().unwrap()
        };
        current_valve
            .tunnels
            .iter()
            .filter(|t| t != &previous_valve)
            .for_each(|t| {
                let mut new_path = path.clone();
                new_path.push(t.to_owned());
                output_paths.push(new_path);
            });
    }
    output_paths
}

fn best_path_to_valve(
    current_position: &str,
    target_valve: &str,
    map: &HashMap<String, input::Valve>,
) -> Vec<String> {
    let mut paths = vec![vec![current_position.to_owned()]];
    while !paths.iter().any(|p| p.last().unwrap() == target_valve) {
        paths = paths_to_valve_next_steps(&paths, map);
    }
    (*paths
        .iter()
        .filter(|&p| p.last().unwrap() == target_valve)
        .collect::<Vec<&Vec<String>>>()
        .first()
        .unwrap())
    .clone()
}

fn get_possible_targets(map: &HashMap<String, input::Valve>) -> Vec<String> {
    map.values()
        .filter(|&v| !v.opened && v.flow_rate > 0)
        .map(|v| v.name.to_owned())
        .collect::<Vec<String>>()
}

fn get_next_steps(
    moves: &[StepAction],
    map: &HashMap<String, input::Valve>,
) -> Vec<Vec<StepAction>> {
    let mut next_possible_moves = match moves.last().unwrap() {
        StepAction::Finished => {
            let mut new_moves = moves.to_owned().to_vec();
            new_moves.push(StepAction::Finished);
            return vec![new_moves];
        }
        StepAction::OpenValve(v) | StepAction::Move(v) => {
            let next_targets = get_possible_targets(map);
            if next_targets.is_empty() {
                let mut new_moves = moves.to_owned().to_vec();
                new_moves.push(StepAction::Finished);
                return vec![new_moves];
            }
            next_targets
                .iter()
                .map(|t| {
                    let path = best_path_to_valve(v, t, map);
                    let new_move = if path.len() == 1 {
                        StepAction::OpenValve(t.to_owned())
                    } else {
                        StepAction::Move(path.get(1).unwrap().to_owned())
                    };
                    let mut new_moves = moves.to_owned().to_vec();
                    new_moves.push(new_move);
                    new_moves
                })
                .collect::<Vec<Vec<StepAction>>>()
        }
    };
    next_possible_moves.sort_by(|a, b| {
        let a_last = a.last().unwrap();
        let b_last = b.last().unwrap();
        match (a_last, b_last) {
            (StepAction::Finished, StepAction::Finished) => std::cmp::Ordering::Equal,
            (StepAction::OpenValve(_), StepAction::Finished)
            | (StepAction::Move(_), StepAction::Finished) => std::cmp::Ordering::Less,
            (StepAction::Finished, StepAction::OpenValve(_))
            | (StepAction::Finished, StepAction::Move(_)) => std::cmp::Ordering::Greater,
            (StepAction::OpenValve(l), StepAction::Move(r))
            | (StepAction::Move(l), StepAction::OpenValve(r))
            | (StepAction::OpenValve(l), StepAction::OpenValve(r))
            | (StepAction::Move(l), StepAction::Move(r)) => l.cmp(r),
        }
    });
    next_possible_moves.dedup();
    next_possible_moves
}

#[derive(Clone)]
struct StepsAndTheirMap {
    steps: Vec<Vec<StepAction>>,
    map: HashMap<String, input::Valve>,
    pressure_released: u32,
    pressure_being_released: u32,
}

fn clone_map(map: &HashMap<String, input::Valve>) -> HashMap<String, input::Valve> {
    let mut output = HashMap::new();
    map.iter().for_each(|(k, v)| {
        output.insert(k.to_owned(), v.clone());
    });
    output
}

fn get_max_length_of_all_moves(all_moves: &mut [StepsAndTheirMap]) -> Vec<StepsAndTheirMap> {
    if all_moves.len() > MAX_ITEMS {
        all_moves.sort_by(|a, b| b.pressure_released.cmp(&a.pressure_released));
        return all_moves[0..MAX_ITEMS].to_vec();
    }
    all_moves.to_vec()
}

fn is_moving_back_and_forth(steps: &Vec<StepAction>) -> bool {
    if steps.len() < 3 {
        return false;
    }
    let last = steps.last().unwrap();
    let second_last = steps.get(steps.len() - 2).unwrap();
    let third_last = steps.get(steps.len() - 3).unwrap();
    if let (StepAction::Move(l), StepAction::Move(_), StepAction::Move(tl)) =
        (last, second_last, third_last)
    {
        if l == tl {
            return true;
        }
    }
    false
}

const MAX_ITEMS: usize = 500;
pub fn get_max_pressure_within_minutes(minutes: u32, map: &HashMap<String, input::Valve>) -> u32 {
    let mut all_moves = vec![StepsAndTheirMap {
        steps: vec![vec![StepAction::Move("AA".to_owned())]],
        map: clone_map(map),
        pressure_released: 0,
        pressure_being_released: 0,
    }];
    for _ in 1..=minutes {
        all_moves = all_moves
            .iter()
            .flat_map(|m| {
                let next_steps = get_next_steps(&m.steps[0], &m.map);
                next_steps
                    .iter()
                    .map(|s| {
                        let mut map = clone_map(&m.map);
                        let mut pressure_being_released = m.pressure_being_released;
                        if let StepAction::OpenValve(opened) = s.last().unwrap() {
                            map.get_mut(opened).unwrap().opened = true;
                            pressure_being_released += map.get(opened).unwrap().flow_rate;
                        }
                        StepsAndTheirMap {
                            steps: vec![s.to_owned()],
                            map,
                            pressure_being_released,
                            pressure_released: m.pressure_released + m.pressure_being_released,
                        }
                    })
                    .collect::<Vec<StepsAndTheirMap>>()
            })
            .collect::<Vec<StepsAndTheirMap>>();
        all_moves = all_moves
            .iter()
            .filter(|m| {
                if is_moving_back_and_forth(&m.steps[0]) {
                    return false;
                }
                true
            })
            .map(|m| m.to_owned())
            .collect::<Vec<StepsAndTheirMap>>();
        all_moves = get_max_length_of_all_moves(&mut all_moves);
    }
    all_moves.sort_by(|a, b| b.pressure_released.cmp(&a.pressure_released));
    all_moves.first().unwrap().pressure_released
}

fn combine_steps(
    your_steps: &Vec<Vec<StepAction>>,
    elephant_steps: &Vec<Vec<StepAction>>,
) -> Vec<Vec<Vec<StepAction>>> {
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

pub fn get_max_pressure_for_two(minutes: u32, map: &HashMap<String, input::Valve>) -> u32 {
    let mut all_moves = vec![StepsAndTheirMap {
        steps: vec![
            vec![StepAction::Move("AA".to_owned())],
            vec![StepAction::Move("AA".to_owned())],
        ],
        map: clone_map(map),
        pressure_released: 0,
        pressure_being_released: 0,
    }];
    for _ in 1..=minutes {
        all_moves = all_moves
            .iter()
            .flat_map(|m| {
                let your_next_steps = get_next_steps(&m.steps[0], &m.map);
                let elephant_next_steps = get_next_steps(&m.steps[1], &m.map);
                let next_steps = combine_steps(&your_next_steps, &elephant_next_steps);
                next_steps
                    .iter()
                    .map(|s| {
                        let mut pressure_being_released = m.pressure_being_released;
                        let mut map = clone_map(&m.map);
                        if let StepAction::OpenValve(opened1) = s[0].last().unwrap() {
                            let opened_valve = map.get_mut(opened1).unwrap();
                            if !opened_valve.opened {
                                opened_valve.opened = true;
                                pressure_being_released += opened_valve.flow_rate;
                            }
                        }
                        if let StepAction::OpenValve(opened2) = s[1].last().unwrap() {
                            let opened_valve = map.get_mut(opened2).unwrap();
                            if !opened_valve.opened {
                                opened_valve.opened = true;
                                pressure_being_released += opened_valve.flow_rate;
                            }
                        }
                        StepsAndTheirMap {
                            steps: s.to_owned(),
                            map,
                            pressure_being_released,
                            pressure_released: m.pressure_released + m.pressure_being_released,
                        }
                    })
                    .collect::<Vec<StepsAndTheirMap>>()
            })
            .collect::<Vec<StepsAndTheirMap>>();
        all_moves = all_moves
            .iter()
            .filter(|m| {
                if is_moving_back_and_forth(&m.steps[0]) {
                    return false;
                }
                if is_moving_back_and_forth(&m.steps[1]) {
                    return false;
                }
                true
            })
            .map(|m| m.to_owned())
            .collect::<Vec<StepsAndTheirMap>>();
        all_moves = get_max_length_of_all_moves(&mut all_moves);
    }
    all_moves.sort_by(|a, b| b.pressure_released.cmp(&a.pressure_released));
    all_moves.first().unwrap().pressure_released
}

#[cfg(test)]
pub mod tests {
    use super::*;

    #[test]
    fn paths_to_valve_01() {
        let expected = vec!["AA".to_owned(), "DD".to_owned(), "EE".to_owned()];
        let map = input::read_input(input::tests::INPUT);
        let result = best_path_to_valve("AA", "EE", &map);
        assert_eq!(expected, result);
    }

    #[test]
    fn paths_to_valve_02() {
        let expected = vec!["AA".to_owned()];
        let map = input::read_input(input::tests::INPUT);
        let result = best_path_to_valve("AA", "AA", &map);
        assert_eq!(expected, result);
    }

    #[test]
    fn get_possible_targets_01() {
        let expected = vec![
            "EE".to_owned(),
            "CC".to_owned(),
            "BB".to_owned(),
            "JJ".to_owned(),
            "HH".to_owned(),
            "DD".to_owned(),
        ];
        let map = input::read_input(input::tests::INPUT);
        let result = get_possible_targets(&map);
        assert_eq!(expected.len(), result.len());
        for e in expected {
            assert!(result.contains(&e));
        }
    }

    #[test]
    fn get_next_steps_01() {
        let expected = vec![
            vec![
                StepAction::Move("AA".to_owned()),
                StepAction::Move("BB".to_owned()),
            ],
            vec![
                StepAction::Move("AA".to_owned()),
                StepAction::Move("DD".to_owned()),
            ],
            vec![
                StepAction::Move("AA".to_owned()),
                StepAction::Move("II".to_owned()),
            ],
        ];
        let mut map = input::read_input(input::tests::INPUT);
        let input = vec![StepAction::Move("AA".to_owned())];
        let result = get_next_steps(&input, &mut map);
        assert_eq!(expected.len(), result.len());
        for e in expected {
            assert!(result.contains(&e));
        }
    }

    #[test]
    fn get_next_steps_02() {
        let expected = vec![
            vec![
                StepAction::Move("EE".to_owned()),
                StepAction::OpenValve("EE".to_owned()),
            ],
            vec![
                StepAction::Move("EE".to_owned()),
                StepAction::Move("FF".to_owned()),
            ],
            vec![
                StepAction::Move("EE".to_owned()),
                StepAction::Move("DD".to_owned()),
            ],
        ];
        let mut map = input::read_input(input::tests::INPUT);
        let input = vec![StepAction::Move("EE".to_owned())];
        let result = get_next_steps(&input, &mut map);
        assert_eq!(expected.len(), result.len());
        for e in expected {
            assert!(result.contains(&e));
        }
    }

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
