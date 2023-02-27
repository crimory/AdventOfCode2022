use crate::day18::domain;

pub fn get_potential_neighbors(target: &domain::Coordinates) -> Vec<domain::Coordinates> {
    vec![
        domain::Coordinates {
            z: target.z + 1,
            ..*target
        },
        domain::Coordinates {
            z: target.z - 1,
            ..*target
        },
        domain::Coordinates {
            y: target.y + 1,
            ..*target
        },
        domain::Coordinates {
            y: target.y - 1,
            ..*target
        },
        domain::Coordinates {
            x: target.x + 1,
            ..*target
        },
        domain::Coordinates {
            x: target.x - 1,
            ..*target
        },
    ]
}

pub fn get_number_of_neighbors(
    target: &domain::Coordinates,
    input: &[domain::Coordinates],
) -> usize {
    let potential_neighbors = get_potential_neighbors(target);
    potential_neighbors
        .iter()
        .filter(|neighbor| input.contains(neighbor))
        .count()
}

#[cfg(test)]
mod tests {
    use super::*;
    use crate::day18::input;

    #[test]
    fn number_of_neighbours_0() {
        let input = input::tests::get_expected_coords();
        let result = get_number_of_neighbors(&input[0], &input);
        assert_eq!(result, 6);
    }

    #[test]
    fn number_of_neighbours_1() {
        let input = input::tests::get_expected_coords();
        let result = get_number_of_neighbors(&input[1], &input);
        assert_eq!(result, 1);
    }
}
