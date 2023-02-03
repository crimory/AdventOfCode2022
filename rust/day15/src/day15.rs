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
    Sensor {
        x: raw_coordinates[0],
        y: raw_coordinates[1],
        closest_beacon: Beacon {
            x: raw_coordinates[2],
            y: raw_coordinates[3],
        },
    }
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
    let min_x = values_x.iter() .min().unwrap();
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

fn get_length_to_point(x: i32, y: i32, sensor: &Sensor) -> i32 {
    let x_diff = (sensor.x - x).abs();
    let y_diff = (sensor.y - y).abs();
    x_diff + y_diff
}

fn get_length_to_closest_beacon(sensor: &Sensor) -> i32 {
    get_length_to_point(sensor.closest_beacon.x, sensor.closest_beacon.y, sensor)
}

fn is_position_occupied_and_not_a_beacon(x: i32, y: i32, map: &[Sensor]) -> bool {
    for sensor in map {
        if sensor.closest_beacon.x == x && sensor.closest_beacon.y == y {
            return false;
        }
        let length_to_point = get_length_to_point(x, y, sensor);
        let length_to_closest_beacon = get_length_to_closest_beacon(sensor);
        if length_to_point <= length_to_closest_beacon {
            return true;
        }
    }
    false
}

pub fn get_occupied_positions(y: i32, input: &str) -> i32 {
    let map = read_input(input);
    let map_size = get_map_size(&map);
    (map_size.min_x..=map_size.max_x)
        .map(|x| is_position_occupied_and_not_a_beacon(x, y, &map))
        .filter(|occupied| *occupied)
        .count() as i32
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
        let expected = Sensor {
            x: 2,
            y: 18,
            closest_beacon: Beacon { x: -2, y: 15 },
        };
        let result = read_input_line(INPUT_LINE);
        assert_eq!(result, expected);
    }

    #[test]
    fn read_input_correctly() {
        let expected = vec![
            Sensor {
                x: 2,
                y: 18,
                closest_beacon: Beacon { x: -2, y: 15 },
            },
            Sensor {
                x: 9,
                y: 16,
                closest_beacon: Beacon { x: 10, y: 16 },
            },
            Sensor {
                x: 13,
                y: 2,
                closest_beacon: Beacon { x: 15, y: 3 },
            },
            Sensor {
                x: 12,
                y: 14,
                closest_beacon: Beacon { x: 10, y: 16 },
            },
            Sensor {
                x: 10,
                y: 20,
                closest_beacon: Beacon { x: 10, y: 16 },
            },
            Sensor {
                x: 14,
                y: 17,
                closest_beacon: Beacon { x: 10, y: 16 },
            },
            Sensor {
                x: 8,
                y: 7,
                closest_beacon: Beacon { x: 2, y: 10 },
            },
            Sensor {
                x: 2,
                y: 0,
                closest_beacon: Beacon { x: 2, y: 10 },
            },
            Sensor {
                x: 0,
                y: 11,
                closest_beacon: Beacon { x: 2, y: 10 },
            },
            Sensor {
                x: 20,
                y: 14,
                closest_beacon: Beacon { x: 25, y: 17 },
            },
            Sensor {
                x: 17,
                y: 20,
                closest_beacon: Beacon { x: 21, y: 22 },
            },
            Sensor {
                x: 16,
                y: 7,
                closest_beacon: Beacon { x: 15, y: 3 },
            },
            Sensor {
                x: 14,
                y: 3,
                closest_beacon: Beacon { x: 15, y: 3 },
            },
            Sensor {
                x: 20,
                y: 1,
                closest_beacon: Beacon { x: 15, y: 3 },
            },
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
        let sensor = Sensor {
            x: 1,
            y: 2,
            closest_beacon: Beacon { x: 0, y: 0 },
        };
        let result = get_length_to_point(0, 0, &sensor);
        assert_eq!(3, result);
    }

    #[test]
    fn get_length_to_closest_beacon_01_correctly() {
        let sensor = Sensor {
            x: 1,
            y: 2,
            closest_beacon: Beacon { x: -5, y: 0 },
        };
        let result = get_length_to_closest_beacon(&sensor);
        assert_eq!(8, result);
    }

    #[test]
    fn is_position_occupied_true_correctly() {
        let map = read_input(INPUT);
        let result = is_position_occupied_and_not_a_beacon(-1, 9, &map);
        assert!(result);
    }

    #[test]
    fn is_position_occupied_false_correctly() {
        let map = read_input(INPUT);
        let result = is_position_occupied_and_not_a_beacon(-2, 9, &map);
        assert!(!result);
    }

    #[test]
    fn is_position_occupied_false_as_beacon_correctly() {
        let map = read_input(INPUT);
        let result = is_position_occupied_and_not_a_beacon(2, 10, &map);
        assert!(!result);
    }

    #[test]
    fn get_occupied_positions_correctly() {
        let result = get_occupied_positions(10, INPUT);
        assert_eq!(26, result)
    }
}
