fn get_input_for_tests() -> String {
    "498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9"
        .to_string()
}

#[derive(Clone, Copy, Debug, PartialEq)]
struct Point {
    x: u32,
    y: u32,
}

#[derive(Debug, PartialEq)]
struct Line {
    start: Point,
    end: Point,
}

#[derive(Clone, Copy, PartialEq)]
enum SandState {
    Falling,
    Settled,
}

#[derive(Clone, Copy, PartialEq)]
struct SandUnit {
    position: Point,
    state: SandState,
}

fn parse_point(point: &str) -> Point {
    // point.split(",")
    //     .collect::<Vec<&str>>()
    //     .windows(2)
    //     .map(|window| {
    //         let x = window[0].parse::<u32>().unwrap();
    //         let y = window[1].parse::<u32>().unwrap();
    //         Point { x, y }
    //     })
    //     .collect::<Vec<Point>>()
    //     .first()
    //     .unwrap()
    //     .to_owned()
    let mut parts = point.split(",");
    let x = parts.next().unwrap().parse::<u32>().unwrap();
    let y = parts.next().unwrap().parse::<u32>().unwrap();
    Point { x, y }
}

fn parse_line(line: &str) -> Vec<Line> {
    line.split(" -> ")
        .collect::<Vec<&str>>()
        .windows(2)
        .map(|window| {
            let start = parse_point(window[0]);
            let end = parse_point(window[1]);
            Line { start, end }
        })
        .collect::<Vec<Line>>()
}

fn parse_input(input: &str) -> Vec<Line> {
    input
        .lines()
        .map(|line| parse_line(line))
        .flatten()
        .collect::<Vec<Line>>()
}

#[test]
fn test_parse_input() {
    let input = get_input_for_tests();
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
    let result = parse_input(&input);
    assert_eq!(expected.len(), result.len());
    for i in 0..expected.len() {
        assert_eq!(expected[i], result[i]);
    }
}

fn get_line_points(line: &Line) -> Vec<Point> {
    let mut points = Vec::<Point>::new();
    if line.start.x == line.end.x {
        let possible_y = if line.start.y < line.end.y {
            (line.start.y..=line.end.y).collect::<Vec<u32>>()
        } else {
            (line.end.y..=line.start.y).rev().collect::<Vec<u32>>()
        };
        possible_y.iter().for_each(|&y| {
            points.push(Point { x: line.start.x, y });
        });
    }
    if line.start.y == line.end.y {
        let possible_x = if line.start.x < line.end.x {
            (line.start.x..=line.end.x).collect::<Vec<u32>>()
        } else {
            (line.end.x..=line.start.x).rev().collect::<Vec<u32>>()
        };
        possible_x.iter().for_each(|&x| {
            points.push(Point { x, y: line.start.y });
        });
    }
    points
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

fn get_points_map(input: &str) -> Vec<Point> {
    let mut points = parse_input(input)
        .iter()
        .map(|line| get_line_points(line))
        .flatten()
        .collect::<Vec<Point>>();
    points.dedup();
    points
}

#[test]
fn test_get_points_map() {
    let input = get_input_for_tests();
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
    let result = get_points_map(&input);
    assert_eq!(expected.len(), result.len());
    for i in 0..expected.len() {
        assert_eq!(expected[i], result[i]);
    }
}

fn get_proper_map(input: Vec<Point>) -> Vec<Vec<bool>> {
    let map_max_x = input.iter().max_by_key(|point| point.x).unwrap().x;
    let map_max_y = input.iter().max_by_key(|point| point.y).unwrap().y;
    let mut test2 = vec![vec![false; (map_max_y + 1) as usize]; (map_max_x + 1) as usize];
    input.iter().for_each(|point| {
        test2[point.x as usize][point.y as usize] = true;
    });
    test2
}

fn get_sand_unit_next_state(map: &Vec<Vec<bool>>, sand_unit: SandUnit) -> SandUnit {
    match sand_unit.state {
        SandState::Settled => sand_unit,
        SandState::Falling => {
            let next_position_below = Point {
                x: sand_unit.position.x,
                y: sand_unit.position.y + 1,
            };
            let next_position_left = Point {
                x: sand_unit.position.x - 1,
                y: sand_unit.position.y + 1,
            };
            let next_position_right = Point {
                x: sand_unit.position.x + 1,
                y: sand_unit.position.y + 1,
            };

            if next_position_below.x >= map.len() as u32
                || next_position_below.y >= map[0].len() as u32
            {
                return SandUnit {
                    position: next_position_below,
                    state: sand_unit.state,
                };
            }

            let is_position_below_settled =
                map[next_position_below.x as usize][next_position_below.y as usize];
            let is_position_left_settled =
                map[next_position_left.x as usize][next_position_left.y as usize];
            let is_position_right_settled =
                map[next_position_right.x as usize][next_position_right.y as usize];

            match (
                is_position_below_settled,
                is_position_left_settled,
                is_position_right_settled,
            ) {
                (true, true, true) => SandUnit {
                    position: sand_unit.position,
                    state: SandState::Settled,
                },
                (false, _, _) => SandUnit {
                    position: next_position_below,
                    state: SandState::Falling,
                },
                (true, false, _) => SandUnit {
                    position: next_position_left,
                    state: SandState::Falling,
                },
                (true, true, false) => SandUnit {
                    position: next_position_right,
                    state: SandState::Falling,
                },
            }
        }
    }
}

fn count_sand_units_that_settle<F>(
    while_condition: F,
    sand_starting: SandUnit,
    mut map: Vec<Vec<bool>>,
) -> u32
where
    F: Fn(&SandUnit) -> bool,
{
    let mut sand_unit = sand_starting;
    let mut sand_units_count = 0u32;
    while while_condition(&sand_unit) || sand_units_count == 0 {
        sand_unit = sand_starting;
        while sand_unit.state == SandState::Falling
            && sand_unit.position.y < (map[0].len() - 1) as u32
        {
            sand_unit = get_sand_unit_next_state(&map, sand_unit);
        }
        match sand_unit.state {
            SandState::Settled => {
                sand_units_count += 1;
                map[sand_unit.position.x as usize][sand_unit.position.y as usize] = true;
            }
            SandState::Falling => {
                continue;
            }
        }
    }
    sand_units_count
}

pub fn get_number_of_sand_units_that_settle(input: &str) -> u32 {
    let raw_map = get_points_map(input);
    let map = get_proper_map(raw_map);
    let starting_sand_unit = SandUnit {
        position: Point { x: 500, y: 0 },
        state: SandState::Falling,
    };
    fn while_condition(sand_unit: &SandUnit) -> bool {
        sand_unit.state == SandState::Settled
    }
    count_sand_units_that_settle(while_condition, starting_sand_unit, map)
}

#[test]
fn test_get_number_of_sand_units_that_settle() {
    let input = get_input_for_tests();
    let result = get_number_of_sand_units_that_settle(&input);
    assert_eq!(24, result);
}

pub fn get_number_of_sand_units_that_settle_with_floor(input: &str) -> u32 {
    let mut raw_map = get_points_map(input);
    let starting_sand_unit = SandUnit {
        position: Point { x: 500, y: 0 },
        state: SandState::Falling,
    };
    let map_max_y = raw_map.iter().max_by_key(|point| point.y).unwrap().y;
    let far_left_floor_point = Point {
        x: starting_sand_unit.position.x - map_max_y - 2,
        y: map_max_y + 2,
    };
    let far_right_floor_point = Point {
        x: starting_sand_unit.position.x + map_max_y + 2,
        y: map_max_y + 2,
    };
    let floor_points = get_line_points(&Line {
        start: far_left_floor_point,
        end: far_right_floor_point,
    });
    raw_map.extend(floor_points);

    let map = get_proper_map(raw_map);
    let while_condition = |sand_unit: &SandUnit| -> bool {
        !(sand_unit.state == SandState::Settled
            && sand_unit.position == starting_sand_unit.position)
    };
    count_sand_units_that_settle(while_condition, starting_sand_unit, map)
}

#[test]
fn test_get_number_of_sand_units_that_settle_with_floor() {
    let input = get_input_for_tests();
    let result = get_number_of_sand_units_that_settle_with_floor(&input);
    assert_eq!(93, result);
}
