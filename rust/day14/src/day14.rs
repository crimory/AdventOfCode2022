mod unit_tests;

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

fn get_points_map(input: &str) -> Vec<Point> {
    let mut points = parse_input(input)
        .iter()
        .map(|line| get_line_points(line))
        .flatten()
        .collect::<Vec<Point>>();
    points.dedup();
    points
}

fn get_proper_map(input: Vec<Point>) -> Vec<Vec<bool>> {
    let map_max_x = input.iter().max_by_key(|point| point.x).unwrap().x;
    let map_max_y = input.iter().max_by_key(|point| point.y).unwrap().y;
    let mut map = vec![vec![false; (map_max_y + 1) as usize]; (map_max_x + 1) as usize];
    input.iter().for_each(|point| {
        map[point.x as usize][point.y as usize] = true;
    });
    map
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
