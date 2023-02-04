#[derive(Debug, PartialEq)]
struct Beacon {
    x: i32,
    y: i32,
}

#[derive(Debug, PartialEq)]
struct Sensor {
    x: i32,
    y: i32,
    closest_beacon: Beacon,
    length_to_closest_beacon: i32,
}
impl Sensor {
    fn new(sensor_x: i32, sensor_y: i32, beacon_x: i32, beacon_y: i32) -> Sensor {
        Sensor {
            x: sensor_x,
            y: sensor_y,
            closest_beacon: Beacon {
                x: beacon_x,
                y: beacon_y,
            },
            length_to_closest_beacon: get_length_between_points(
                sensor_x, sensor_y, beacon_x, beacon_y,
            ),
        }
    }
}

enum MapSizeDimension {
    X,
    Y,
}

#[derive(Debug, PartialEq)]
struct MapSize {
    min_x: i32,
    min_y: i32,
    max_x: i32,
    max_y: i32,
}

fn read_input_line(line: &str) -> Sensor {
    let raw_coordinates = line
        .split(',')
        .flat_map(|s| s.trim().split(':'))
        .map(|s| s.trim().split('='))
        .flat_map(|s| s.skip(1))
        .map(|s| s.trim().parse::<i32>().unwrap())
        .collect::<Vec<i32>>();
    Sensor::new(
        raw_coordinates[0],
        raw_coordinates[1],
        raw_coordinates[2],
        raw_coordinates[3],
    )
}

fn read_input(input: &str) -> Vec<Sensor> {
    input.lines().map(read_input_line).collect()
}

fn get_map_sizes_by_dimension(map: &[Sensor], dimension: MapSizeDimension) -> Vec<i32> {
    let mut sensor_values = match dimension {
        MapSizeDimension::X => map.iter().map(|sensor| sensor.x).collect::<Vec<i32>>(),
        MapSizeDimension::Y => map.iter().map(|sensor| sensor.y).collect::<Vec<i32>>(),
    };
    let beacon_values = map
        .iter()
        .map(|sensor| match dimension {
            MapSizeDimension::X => sensor.closest_beacon.x,
            MapSizeDimension::Y => sensor.closest_beacon.y,
        })
        .collect::<Vec<i32>>();
    sensor_values.extend(beacon_values);
    sensor_values
}

fn get_map_size(map: &[Sensor]) -> MapSize {
    let values_x = get_map_sizes_by_dimension(map, MapSizeDimension::X);
    let values_y = get_map_sizes_by_dimension(map, MapSizeDimension::Y);
    let min_x = values_x.iter().min().unwrap();
    let min_y = values_y.iter().min().unwrap();
    let max_x = values_x.iter().max().unwrap();
    let max_y = values_y.iter().max().unwrap();
    MapSize {
        min_x: *min_x,
        min_y: *min_y,
        max_x: *max_x,
        max_y: *max_y,
    }
}

fn get_length_between_points(x_1: i32, y_1: i32, x_2: i32, y_2: i32) -> i32 {
    let x_diff = (x_2 - x_1).abs();
    let y_diff = (y_2 - y_1).abs();
    x_diff + y_diff
}

fn get_length_to_point(x: i32, y: i32, sensor: &Sensor) -> i32 {
    get_length_between_points(x, y, sensor.x, sensor.y)
}

fn is_position_a_beacon(x: i32, y: i32, map: &[Sensor]) -> bool {
    for sensor in map {
        if sensor.closest_beacon.x == x && sensor.closest_beacon.y == y {
            return true;
        }
    }
    false
}

fn is_position_occupied(x: i32, y: i32, map: &[Sensor]) -> bool {
    for sensor in map {
        let length_to_point = get_length_to_point(x, y, sensor);
        if length_to_point <= sensor.length_to_closest_beacon {
            return true;
        }
    }
    false
}

pub fn get_occupied_positions_which_are_not_beacons(y: i32, input: &str) -> i32 {
    let map = read_input(input);
    let map_size = get_map_size(&map);
    (map_size.min_x..=map_size.max_x)
        .filter(|&x| !is_position_a_beacon(x, y, &map))
        .map(|x| is_position_occupied(x, y, &map))
        .filter(|occupied| *occupied)
        .count() as i32
}

fn get_sorted_x_ranges_for_specific_y(y: i32, sensors: &[Sensor]) -> Vec<(i32, i32)> {
    let mut ranges = vec![];
    for sensor in sensors {
        let length_left = sensor.length_to_closest_beacon - (y - sensor.y).abs();
        if length_left < 0 {
            continue;
        }
        ranges.push((sensor.x - length_left, sensor.x + length_left))
    }
    ranges.sort_by(|a, b| a.0.cmp(&b.0));
    ranges
}

pub fn get_tuning_frequency_for_distress_beacon(
    search_min: i32,
    search_max: i32,
    input: &str,
) -> i64 {
    let sensors = read_input(input);
    for y in search_min..=search_max {
        let ranges = get_sorted_x_ranges_for_specific_y(y, &sensors);
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

    const INPUT_LINE: &str = "Sensor at x=2, y=18: closest beacon is at x=-2, y=15";
    pub(crate) const INPUT: &str = "\
Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3";

    #[test]
    fn read_line_correctly() {
        let expected = Sensor::new(2, 18, -2, 15);
        let result = read_input_line(INPUT_LINE);
        assert_eq!(result, expected);
    }

    #[test]
    fn read_input_correctly() {
        let expected = vec![
            Sensor::new(2, 18, -2, 15),
            Sensor::new(9, 16, 10, 16),
            Sensor::new(13, 2, 15, 3),
            Sensor::new(12, 14, 10, 16),
            Sensor::new(10, 20, 10, 16),
            Sensor::new(14, 17, 10, 16),
            Sensor::new(8, 7, 2, 10),
            Sensor::new(2, 0, 2, 10),
            Sensor::new(0, 11, 2, 10),
            Sensor::new(20, 14, 25, 17),
            Sensor::new(17, 20, 21, 22),
            Sensor::new(16, 7, 15, 3),
            Sensor::new(14, 3, 15, 3),
            Sensor::new(20, 1, 15, 3),
        ];
        let result = read_input(INPUT);
        assert_eq!(result, expected);
    }

    #[test]
    fn get_map_size_correctly() {
        let expected = MapSize {
            min_x: -2,
            min_y: 0,
            max_x: 25,
            max_y: 22,
        };
        let map = read_input(INPUT);
        let result = get_map_size(&map);
        assert_eq!(result, expected);
    }

    #[test]
    fn get_length_to_point_01_correctly() {
        let sensor = Sensor::new(1, 2, 0, 0);
        let result = get_length_to_point(0, 0, &sensor);
        assert_eq!(3, result);
    }

    #[test]
    fn is_position_occupied_true_correctly() {
        let map = read_input(INPUT);
        let result = is_position_occupied(-1, 9, &map);
        assert!(result);
    }

    #[test]
    fn is_position_occupied_false_correctly() {
        let map = read_input(INPUT);
        let result = is_position_occupied(-2, 9, &map);
        assert!(!result);
    }

    #[test]
    fn is_position_a_beacon_correctly() {
        let map = read_input(INPUT);
        let result = is_position_a_beacon(2, 10, &map);
        assert!(result);
    }

    #[test]
    fn get_occupied_positions_correctly() {
        let result = get_occupied_positions_which_are_not_beacons(10, INPUT);
        assert_eq!(26, result)
    }

    #[test]
    fn get_tuning_frequency_for_distress_beacon_correctly() {
        let result = get_tuning_frequency_for_distress_beacon(0, 20, INPUT);
        assert_eq!(56_000_011, result)
    }
}
