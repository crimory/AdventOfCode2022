mod day15;

fn main() {
    let input = include_str!("input.txt");

    let y = 2_000_000;
    let number_of_occupied_positions =
        day15::get_occupied_positions_which_are_not_beacons(y, input);
    println!("Number of positions on row {y}, that cannot contain a beacon: {number_of_occupied_positions}");

    let tuning_frequency = day15::get_tuning_frequency_for_distress_beacon(0, 4_000_000, input);
    println!("Tuning frequency of distress beacon: {tuning_frequency}");
}
