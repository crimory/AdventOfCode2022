mod domain;
mod fill;
mod input;
mod neighbor;

fn number_of_exposed_sides(input: &[domain::Coordinates]) -> usize {
    input
        .iter()
        .map(|coord| neighbor::get_number_of_neighbors(coord, input))
        .map(|number_of_neighbors| 6 - number_of_neighbors)
        .sum::<usize>()
}

pub fn get_number_of_exposed_sides(text: &str) -> usize {
    let input = input::read_input(text);
    number_of_exposed_sides(&input)
}

pub fn get_number_of_outer_exposed_sides(text: &str) -> usize {
    let input = input::read_input(text);
    let negative_shape =
        fill::get_negative_shape_from_outside(&input, neighbor::get_potential_neighbors);
    let all_exposed = number_of_exposed_sides(&negative_shape.negative_shape);
    all_exposed - negative_shape.outer_shell_size
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn exposed_sides() {
        let result = get_number_of_exposed_sides(input::tests::INPUT);
        assert_eq!(result, 64);
    }

    #[test]
    fn exposed_outer_sides() {
        let result = get_number_of_outer_exposed_sides(input::tests::INPUT);
        assert_eq!(result, 58);
    }
}
