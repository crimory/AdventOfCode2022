#[derive(Debug, PartialEq)]
pub struct Beacon {
    pub x: i32,
    pub y: i32,
}

#[derive(Debug, PartialEq)]
pub struct Sensor {
    pub x: i32,
    pub y: i32,
    pub closest_beacon: Beacon,
    pub length_to_closest_beacon: i32,
}
impl Sensor {
    pub fn new(sensor_x: i32, sensor_y: i32, beacon_x: i32, beacon_y: i32) -> Sensor {
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

pub fn get_length_between_points(x_1: i32, y_1: i32, x_2: i32, y_2: i32) -> i32 {
    let x_diff = (x_2 - x_1).abs();
    let y_diff = (y_2 - y_1).abs();
    x_diff + y_diff
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

pub fn read_input(input: &str) -> Vec<Sensor> {
    input.lines().map(read_input_line).collect()
}

#[cfg(test)]
pub mod tests {
    use super::*;

    const INPUT_LINE: &str = "Sensor at x=2, y=18: closest beacon is at x=-2, y=15";
    pub const INPUT: &str = "\
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
}