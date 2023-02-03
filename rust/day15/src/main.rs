mod day15;

fn main() {
    let input = include_str!("input.txt");

    let y = 2_000_000;
    let number_of_occupied_positions = day15::get_occupied_positions(y, input);
    println!("Number of positions on row {y}, that cannot contain a beacon: {number_of_occupied_positions}");
}
