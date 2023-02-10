mod find;
mod input;

pub fn most_pressure_released(input: &str) -> u32 {
    let map = input::read_input(input);
    find::get_max_pressure_within_minutes(30, &map)
}

pub fn most_pressure_released_with_two_actors(input: &str) -> u32 {
    let map = input::read_input(input);
    find::get_max_pressure_for_two(26, &map)
}
