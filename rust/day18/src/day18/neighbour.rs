use crate::day18::input;

pub fn get_number_of_neighbours(target: &input::Coordinates, input: &Vec<input::Coordinates>) -> usize {
    let potential_neighbours = vec![
        input::Coordinates {
            x: target.x,
            y: target.y,
            z: target.z + 1,
        },
        input::Coordinates {
            x: target.x,
            y: target.y,
            z: target.z - 1,
        },
        input::Coordinates {
            x: target.x,
            y: target.y + 1,
            z: target.z,
        },
        input::Coordinates {
            x: target.x,
            y: target.y - 1,
            z: target.z,
        },
        input::Coordinates {
            x: target.x + 1,
            y: target.y,
            z: target.z,
        },
        input::Coordinates {
            x: target.x - 1,
            y: target.y,
            z: target.z,
        },
    ];
    potential_neighbours
        .iter()
        .filter(|neighbour| input.contains(neighbour))
        .count()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn number_of_neighbours_0() {
        let input = input::tests::get_expected_coords();
        let result = get_number_of_neighbours(&input[0], &input);
        assert_eq!(result, 6);
    }

    #[test]
    fn number_of_neighbours_1() {
        let input = input::tests::get_expected_coords();
        let result = get_number_of_neighbours(&input[1], &input);
        assert_eq!(result, 1);
    }
}