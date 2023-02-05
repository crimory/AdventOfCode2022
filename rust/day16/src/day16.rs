mod directions;
mod input;

pub fn most_pressure_released(input: &str) -> u32 {
    let map = input::read_input(input);
    directions::get_max_pressure_within_minutes(30, &map)
}
