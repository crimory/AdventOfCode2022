mod day16;
mod day16_2;

fn main() {
    let input = include_str!("input.txt");

    let pressure_released = day16_2::most_pressure_released(input);
    println!("Pressure released: {pressure_released}");

    let pressure_released = day16_2::most_pressure_released_with_two_actors(input);
    println!("Pressure released for 2: {pressure_released}");
}
