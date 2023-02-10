use crate::day16_2::{find, input};

fn paths_to_valve_next_steps(
    paths: &Vec<Vec<u32>>,
    map: &input::Map,
) -> Vec<Vec<u32>> {
    let mut output_paths = Vec::new();
    for path in paths {
        let current_index = path.last().unwrap();
        let current_valve = map.get_valve_by_index(current_index);
        let previous_valve = if path.len() > 1 {
            path.get(path.len() - 2).unwrap()
        } else {
            path.last().unwrap()
        };
        current_valve
            .tunnel_indices
            .iter()
            .filter(|t| *t != previous_valve)
            .for_each(|t| {
                let mut new_path = path.clone();
                new_path.push(*t);
                output_paths.push(new_path);
            });
    }
    output_paths
}

fn best_path_to_valve(
    current_position: &u32,
    target_valve: &u32,
    map: &input::Map,
) -> Vec<u32> {
    let mut paths = vec![vec![*current_position]];
    while !paths.iter().any(|p| p.last().unwrap() == target_valve) {
        paths = paths_to_valve_next_steps(&paths.to_vec(), map);
    }
    paths.iter()
        .filter(|p| p.last().unwrap() == target_valve)
        .collect::<Vec<&Vec<u32>>>()
        .first()
        .unwrap()
        .to_vec()
}

pub fn get_next_steps(
    moves: &[find::StepAction],
    valves_left_to_open: &Vec<u32>,
    map: &input::Map,
) -> Vec<Vec<find::StepAction>> {
    let mut next_possible_moves = match moves.last().unwrap() {
        find::StepAction::Finished => {
            let mut new_moves = moves.to_vec();
            new_moves.push(find::StepAction::Finished);
            return vec![new_moves];
        }
        find::StepAction::OpenValve(v) | find::StepAction::Move(v) => {
            if valves_left_to_open.is_empty() {
                let mut new_moves = moves.to_vec();
                new_moves.push(find::StepAction::Finished);
                return vec![new_moves];
            }
            valves_left_to_open
                .iter()
                .map(|t| {
                    let path = best_path_to_valve(v, t, map);
                    let new_move = if path.len() == 1 {
                        find::StepAction::OpenValve(*t)
                    } else {
                        find::StepAction::Move(*path.get(1).unwrap())
                    };
                    let mut new_moves = moves.to_vec();
                    new_moves.push(new_move);
                    new_moves
                })
                .collect::<Vec<Vec<find::StepAction>>>()
        }
    };
    next_possible_moves.sort_by(|a, b| {
        let a_last = a.last().unwrap();
        let b_last = b.last().unwrap();
        match (a_last, b_last) {
            (find::StepAction::Finished, find::StepAction::Finished) => std::cmp::Ordering::Equal,
            (find::StepAction::OpenValve(_), find::StepAction::Finished)
            | (find::StepAction::Move(_), find::StepAction::Finished) => std::cmp::Ordering::Less,
            (find::StepAction::Finished, find::StepAction::OpenValve(_))
            | (find::StepAction::Finished, find::StepAction::Move(_)) => std::cmp::Ordering::Greater,
            (find::StepAction::OpenValve(l), find::StepAction::Move(r))
            | (find::StepAction::Move(l), find::StepAction::OpenValve(r))
            | (find::StepAction::OpenValve(l), find::StepAction::OpenValve(r))
            | (find::StepAction::Move(l), find::StepAction::Move(r)) => l.cmp(r),
        }
    });
    next_possible_moves.dedup();
    next_possible_moves
}

pub fn is_moving_back_and_forth(steps: &Vec<find::StepAction>) -> bool {
    if steps.len() < 3 {
        return false;
    }
    let last = steps.last().unwrap();
    let second_last = steps.get(steps.len() - 2).unwrap();
    let third_last = steps.get(steps.len() - 3).unwrap();
    if let (find::StepAction::Move(l), find::StepAction::Move(_), find::StepAction::Move(tl)) =
        (last, second_last, third_last)
    {
        if l == tl {
            return true;
        }
    }
    false
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn paths_to_valve_01() {
        let expected = vec![0, 3, 4];
        let map = input::read_input(input::tests::INPUT);
        let result = best_path_to_valve(&0, &4, &map);
        assert_eq!(expected, result);
    }

    #[test]
    fn paths_to_valve_02() {
        let expected = vec![0];
        let map = input::read_input(input::tests::INPUT);
        let result = best_path_to_valve(&0, &0, &map);
        assert_eq!(expected, result);
    }

    #[test]
    fn get_next_steps_01() {
        let expected = vec![
            vec![
                find::StepAction::Move(0),
                find::StepAction::Move(1),
            ],
            vec![
                find::StepAction::Move(0),
                find::StepAction::Move(3),
            ],
            vec![
                find::StepAction::Move(0),
                find::StepAction::Move(8),
            ],
        ];
        let map = input::read_input(input::tests::INPUT);
        let input_moves = vec![find::StepAction::Move(0)];
        let input_open_valves = vec![1, 2, 3, 4, 7, 9];
        let result = get_next_steps(&input_moves, &input_open_valves, &map);
        assert_eq!(expected.len(), result.len());
        for e in expected {
            assert!(result.contains(&e));
        }
    }

    #[test]
    fn get_next_steps_02() {
        let expected = vec![
            vec![
                find::StepAction::Move(4),
                find::StepAction::OpenValve(4),
            ],
            vec![
                find::StepAction::Move(4),
                find::StepAction::Move(5),
            ],
            vec![
                find::StepAction::Move(4),
                find::StepAction::Move(3),
            ],
        ];
        let map = input::read_input(input::tests::INPUT);
        let input_moves = vec![find::StepAction::Move(4)];
        let input_open_valves = vec![1, 2, 3, 4, 7, 9];
        let result = get_next_steps(&input_moves, &input_open_valves, &map);
        assert_eq!(expected.len(), result.len());
        for e in expected {
            assert!(result.contains(&e));
        }
    }
}