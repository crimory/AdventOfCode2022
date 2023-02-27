use crate::day18::domain;

pub fn read_input(input: &str) -> Vec<domain::Coordinates> {
    input
        .lines()
        .map(|line| {
            let mut parts = line.split(',');
            let x = parts.next().unwrap().parse::<i32>().unwrap();
            let y = parts.next().unwrap().parse::<i32>().unwrap();
            let z = parts.next().unwrap().parse::<i32>().unwrap();
            domain::Coordinates { x, y, z }
        })
        .collect()
}

#[cfg(test)]
pub mod tests {
    use super::*;

    pub fn get_expected_coords() -> Vec<domain::Coordinates> {
        vec![
            domain::Coordinates { x: 2, y: 2, z: 2 },
            domain::Coordinates { x: 1, y: 2, z: 2 },
            domain::Coordinates { x: 3, y: 2, z: 2 },
            domain::Coordinates { x: 2, y: 1, z: 2 },
            domain::Coordinates { x: 2, y: 3, z: 2 },
            domain::Coordinates { x: 2, y: 2, z: 1 },
            domain::Coordinates { x: 2, y: 2, z: 3 },
            domain::Coordinates { x: 2, y: 2, z: 4 },
            domain::Coordinates { x: 2, y: 2, z: 6 },
            domain::Coordinates { x: 1, y: 2, z: 5 },
            domain::Coordinates { x: 3, y: 2, z: 5 },
            domain::Coordinates { x: 2, y: 1, z: 5 },
            domain::Coordinates { x: 2, y: 3, z: 5 },
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
