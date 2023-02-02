#[cfg(test)]
mod tests {
    use super::super::*;

    const INPUT: &str = "\
498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9";

    #[test]
    fn test_parse_input() {
        let expected = vec![
            Line {
                start: Point { x: 498, y: 4 },
                end: Point { x: 498, y: 6 },
            },
            Line {
                start: Point { x: 498, y: 6 },
                end: Point { x: 496, y: 6 },
            },
            Line {
                start: Point { x: 503, y: 4 },
                end: Point { x: 502, y: 4 },
            },
            Line {
                start: Point { x: 502, y: 4 },
                end: Point { x: 502, y: 9 },
            },
            Line {
                start: Point { x: 502, y: 9 },
                end: Point { x: 494, y: 9 },
            },
        ];
        let result = parse_input(INPUT);
        assert_eq!(expected.len(), result.len());
        for i in 0..expected.len() {
            assert_eq!(expected[i], result[i]);
        }
    }

    #[test]
    fn test_get_line_points() {
        let line = Line {
            start: Point { x: 503, y: 4 },
            end: Point { x: 502, y: 4 },
        };
        let expected = vec![Point { x: 503, y: 4 }, Point { x: 502, y: 4 }];
        let result = get_line_points(&line);
        assert_eq!(expected.len(), result.len());
        for i in 0..expected.len() {
            assert_eq!(expected[i], result[i]);
        }
    }

    #[test]
    fn test_get_points_map() {
        let expected = vec![
            Point { x: 498, y: 4 },
            Point { x: 498, y: 5 },
            Point { x: 498, y: 6 },
            Point { x: 497, y: 6 },
            Point { x: 496, y: 6 },
            Point { x: 503, y: 4 },
            Point { x: 502, y: 4 },
            Point { x: 502, y: 5 },
            Point { x: 502, y: 6 },
            Point { x: 502, y: 7 },
            Point { x: 502, y: 8 },
            Point { x: 502, y: 9 },
            Point { x: 501, y: 9 },
            Point { x: 500, y: 9 },
            Point { x: 499, y: 9 },
            Point { x: 498, y: 9 },
            Point { x: 497, y: 9 },
            Point { x: 496, y: 9 },
            Point { x: 495, y: 9 },
            Point { x: 494, y: 9 },
        ];
        let result = get_points_map(INPUT);
        assert_eq!(expected.len(), result.len());
        for i in 0..expected.len() {
            assert_eq!(expected[i], result[i]);
        }
    }

    #[test]
    fn test_get_number_of_sand_units_that_settle() {
        let result = get_number_of_sand_units_that_settle(INPUT);
        assert_eq!(24, result);
    }

    #[test]
    fn test_get_number_of_sand_units_that_settle_with_floor() {
        let result = get_number_of_sand_units_that_settle_with_floor(INPUT);
        assert_eq!(93, result);
    }
}
