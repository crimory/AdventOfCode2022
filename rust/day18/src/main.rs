mod day18;

fn main() {
    let input = include_str!("input.txt");

    let exposed_sides = day18::get_number_of_exposed_sides(input);
    println!("Number of exposed sides: {exposed_sides}");

    let exposed_sides = day18::get_number_of_outer_exposed_sides(input);
    println!("Number of outer exposed sides: {exposed_sides}");
}
