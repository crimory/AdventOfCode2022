mod input;
mod neighbor;

pub fn get_number_of_exposed_sides(text: &str) -> usize {
    let input = input::read_input(text);
    input
        .iter()
        .map(|coord| neighbor::get_number_of_neighbors(coord, &input))
        .map(|number_of_neighbors| 6 - number_of_neighbors)
        .sum::<usize>()
}

pub fn get_number_of_outer_exposed_sides(text: &str) -> usize {
    0
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
