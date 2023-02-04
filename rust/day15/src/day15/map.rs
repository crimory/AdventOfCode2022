use super::input;

enum MapSizeDimension {
    X,
    Y,
}

#[derive(Debug, PartialEq)]
pub struct MapSize {
    pub min_x: i32,
    pub min_y: i32,
    pub max_x: i32,
    pub max_y: i32,
}

fn get_map_sizes_by_dimension(map: &[input::Sensor], dimension: MapSizeDimension) -> Vec<i32> {
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

pub fn get_map_size(map: &[input::Sensor]) -> MapSize {
    let values_x = get_map_sizes_by_dimension(map, MapSizeDimension::X);
    let values_y = get_map_sizes_by_dimension(map, MapSizeDimension::Y);
    let &min_x = values_x.iter().min().unwrap();
    let &min_y = values_y.iter().min().unwrap();
    let &max_x = values_x.iter().max().unwrap();
    let &max_y = values_y.iter().max().unwrap();
    MapSize {
        min_x,
        min_y,
        max_x,
        max_y,
    }
}

fn get_length_to_point(x: i32, y: i32, sensor: &input::Sensor) -> i32 {
    input::get_length_between_points(x, y, sensor.x, sensor.y)
}

pub fn is_position_a_beacon(x: i32, y: i32, map: &[input::Sensor]) -> bool {
    for sensor in map {
        if sensor.closest_beacon.x == x && sensor.closest_beacon.y == y {
            return true;
        }
    }
    false
}

pub fn is_position_occupied(x: i32, y: i32, map: &[input::Sensor]) -> bool {
    for sensor in map {
        let length_to_point = get_length_to_point(x, y, sensor);
        if length_to_point <= sensor.length_to_closest_beacon {
            return true;
        }
    }
    false
}

pub fn get_sorted_x_ranges_for_specific_y(y: i32, sensors: &[input::Sensor]) -> Vec<(i32, i32)> {
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

#[cfg(test)]
mod tests {
    use super::*;
    use crate::day15::input;

    #[test]
    fn get_map_size_correctly() {
        let expected = MapSize {
            min_x: -2,
            min_y: 0,
            max_x: 25,
            max_y: 22,
        };
        let map = input::read_input(input::tests::INPUT);
        let result = get_map_size(&map);
        assert_eq!(result, expected);
    }

    #[test]
    fn get_length_to_point_01_correctly() {
        let sensor = input::Sensor::new(1, 2, 0, 0);
        let result = get_length_to_point(0, 0, &sensor);
        assert_eq!(3, result);
    }

    #[test]
    fn is_position_occupied_true_correctly() {
        let map = input::read_input(input::tests::INPUT);
        let result = is_position_occupied(-1, 9, &map);
        assert!(result);
    }

    #[test]
    fn is_position_occupied_false_correctly() {
        let map = input::read_input(input::tests::INPUT);
        let result = is_position_occupied(-2, 9, &map);
        assert!(!result);
    }

    #[test]
    fn is_position_a_beacon_correctly() {
        let map = input::read_input(input::tests::INPUT);
        let result = is_position_a_beacon(2, 10, &map);
        assert!(result);
    }
}