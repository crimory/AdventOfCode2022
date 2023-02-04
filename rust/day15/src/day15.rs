mod input;
mod map;

pub fn get_occupied_positions_which_are_not_beacons(y: i32, input: &str) -> i32 {
    let map = input::read_input(input);
    let map_size = map::get_map_size(&map);
    (map_size.min_x..=map_size.max_x)
        .filter(|&x| !map::is_position_a_beacon(x, y, &map))
        .map(|x| map::is_position_occupied(x, y, &map))
        .filter(|occupied| *occupied)
        .count() as i32
}

pub fn get_tuning_frequency_for_distress_beacon(
    search_min: i32,
    search_max: i32,
    input: &str,
) -> i64 {
    let sensors = input::read_input(input);
    for y in search_min..=search_max {
        let ranges = map::get_sorted_x_ranges_for_specific_y(y, &sensors);
        let mut max_x = search_min;
        for range in ranges {
            if range.0 > (max_x + 1) {
                return ((max_x + 1) as i64) * 4_000_000 + (y as i64);
            }
            if range.1 > max_x {
                max_x = range.1;
            }
        }
    }
    0
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn get_occupied_positions_correctly() {
        let result = get_occupied_positions_which_are_not_beacons(10, input::tests::INPUT);
        assert_eq!(26, result)
    }

    #[test]
    fn get_tuning_frequency_for_distress_beacon_correctly() {
        let result = get_tuning_frequency_for_distress_beacon(0, 20, input::tests::INPUT);
        assert_eq!(56_000_011, result)
    }
}
