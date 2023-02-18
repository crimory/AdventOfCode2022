mod input;
mod neighbour;

pub fn get_number_of_exposed_sides(text: &str) -> usize {
    let input = input::read_input(text);
    input
        .iter()
        .map(|coord| neighbour::get_number_of_neighbours(coord, &input))
        .map(|number_of_neighbours| 6 - number_of_neighbours)
        .sum::<usize>()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn exposed_sides() {
        let result = get_number_of_exposed_sides(input::tests::INPUT);
        assert_eq!(result, 64);
    }
}
