#[derive(PartialEq, Eq, Debug)]
pub struct Coordinates {
    pub x: i32,
    pub y: i32,
    pub z: i32,
}

pub fn read_input(input: &str) -> Vec<Coordinates> {
    input
        .lines()
        .map(|line| {
            let mut parts = line.split(',');
            let x = parts.next().unwrap().parse::<i32>().unwrap();
            let y = parts.next().unwrap().parse::<i32>().unwrap();
            let z = parts.next().unwrap().parse::<i32>().unwrap();
            Coordinates { x, y, z }
        })
        .collect()
}

#[cfg(test)]
pub mod tests {
    use super::*;

    pub fn get_expected_coords() -> Vec<Coordinates> {
        vec![
            Coordinates { x: 2, y: 2, z: 2 },
            Coordinates { x: 1, y: 2, z: 2 },
            Coordinates { x: 3, y: 2, z: 2 },
            Coordinates { x: 2, y: 1, z: 2 },
            Coordinates { x: 2, y: 3, z: 2 },
            Coordinates { x: 2, y: 2, z: 1 },
            Coordinates { x: 2, y: 2, z: 3 },
            Coordinates { x: 2, y: 2, z: 4 },
            Coordinates { x: 2, y: 2, z: 6 },
            Coordinates { x: 1, y: 2, z: 5 },
            Coordinates { x: 3, y: 2, z: 5 },
            Coordinates { x: 2, y: 1, z: 5 },
            Coordinates { x: 2, y: 3, z: 5 },
        ]
    }

    pub const INPUT: &str = "\
2,2,2
1,2,2
3,2,2
2,1,2
2,3,2
2,2,1
2,2,3
2,2,4
2,2,6
1,2,5
3,2,5
2,1,5
2,3,5";

    #[test]
    fn test_read_input() {
        let expected = get_expected_coords();
        let result = read_input(INPUT);
        assert_eq!(result, expected);
    }
}
