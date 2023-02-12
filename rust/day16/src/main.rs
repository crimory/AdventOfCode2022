mod day16;
mod day16_2;

enum Version {
    One,
    Two,
}

fn main() {
    let version = Version::Two;
    let input = include_str!("input.txt");

    match version {
        Version::One => {
            let pressure_released = day16::most_pressure_released(input);
            println!("Pressure released v1: {pressure_released}");

            let pressure_released = day16::most_pressure_released_with_two_actors(input);
            println!("Pressure released v1 for 2: {pressure_released}");
        }
        Version::Two => {
            let pressure_released = day16_2::most_pressure_released(input);
            println!("Pressure released v2: {pressure_released}");

            let pressure_released = day16_2::most_pressure_released_with_two_actors(input);
            println!("Pressure released v2 for 2: {pressure_released}");
        }
    }
}
