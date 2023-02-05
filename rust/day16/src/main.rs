mod day16;

fn main() {
    let input = include_str!("input.txt");

    let pressure_released = day16::most_pressure_released(&input);
    println!("Pressure released: {pressure_released}");
}
