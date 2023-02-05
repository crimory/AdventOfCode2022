mod day16;

fn main() {
    let input = include_str!("input.txt");

    let pressure_released = day16::most_pressure_released(input);
    println!("Pressure released: {pressure_released}");

    let pressure_released = day16::most_pressure_released_with_two_actors(input);
    println!("Pressure released for 2: {pressure_released}");
}
