mod day18;

fn main() {
    let input = include_str!("input.txt");

    let tower_height = day18::get_number_of_exposed_sides(input);
    println!("Number of exposed sides: {tower_height}");
}
