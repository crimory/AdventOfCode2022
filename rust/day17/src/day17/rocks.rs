use crate::day17::map;

#[derive(Debug, PartialEq, Clone, Eq, Hash)]
pub struct Point {
    pub x: u64,
    pub y: u64,
}

pub enum Side {
    Left,
    Right,
}
pub enum ShapeMove {
    Sideways(Side),
    Down,
}

#[derive(Debug, PartialEq, Clone)]
pub struct SettledShape {
    pub points: Vec<Point>,
}

#[derive(Debug, PartialEq, Clone)]
pub enum ShapeState {
    Settled(SettledShape),
    Falling(Shape),
}
impl ShapeState {
    pub fn move_shape(&mut self, movement: ShapeMove, map: &map::Map) {
        match self {
            ShapeState::Settled(_) => (),
            ShapeState::Falling(shape) => {
                let mut new_shape = shape.clone();
                match movement {
                    ShapeMove::Sideways(side) => {
                        match side {
                            Side::Left => {
                                new_shape.anchor.x -= 1;
                            }
                            Side::Right => {
                                new_shape.anchor.x += 1;
                            }
                        };
                        if !map.are_points_free(&new_shape) {
                            return;
                        }
                        *self = ShapeState::Falling(new_shape)
                    }
                    ShapeMove::Down => {
                        if shape.anchor.y == 0 {
                            *self = ShapeState::Settled(shape.to_settled());
                            return;
                        }
                        new_shape.anchor.y -= 1;
                        if map.are_points_free(&new_shape) {
                            *self = ShapeState::Falling(new_shape)
                        } else {
                            *self = ShapeState::Settled(shape.to_settled());
                        }
                    }
                }
            }
        }
    }
}

#[derive(Debug, PartialEq, Clone)]
pub struct Shape {
    points: Vec<Point>,
    anchor: Point,
}
impl Shape {
    pub fn to_settled(&self) -> SettledShape {
        SettledShape {
            points: self.get_current_points(),
        }
    }
    pub fn get_current_points(&self) -> Vec<Point> {
        self.points
            .iter()
            .map(|p| Point {
                x: p.x + self.anchor.x,
                y: p.y + self.anchor.y,
            })
            .collect()
    }
}

#[derive(Debug, PartialEq)]
pub enum RockKind {
    Horizontal,
    Plus,
    Corner,
    Vertical,
    Box,
}
impl RockKind {
    pub fn to_shape(&self, anchor: &Point) -> Shape {
        let points = match self {
            RockKind::Horizontal => vec![
                Point { x: 0, y: 0 },
                Point { x: 1, y: 0 },
                Point { x: 2, y: 0 },
                Point { x: 3, y: 0 },
            ],
            RockKind::Plus => vec![
                Point { x: 1, y: 0 },
                Point { x: 0, y: 1 },
                Point { x: 1, y: 1 },
                Point { x: 2, y: 1 },
                Point { x: 1, y: 2 },
            ],
            RockKind::Corner => vec![
                Point { x: 0, y: 0 },
                Point { x: 1, y: 0 },
                Point { x: 2, y: 0 },
                Point { x: 2, y: 1 },
                Point { x: 2, y: 2 },
            ],
            RockKind::Vertical => vec![
                Point { x: 0, y: 0 },
                Point { x: 0, y: 1 },
                Point { x: 0, y: 2 },
                Point { x: 0, y: 3 },
            ],
            RockKind::Box => vec![
                Point { x: 0, y: 0 },
                Point { x: 1, y: 0 },
                Point { x: 0, y: 1 },
                Point { x: 1, y: 1 },
            ],
        };
        Shape {
            points,
            anchor: anchor.clone(),
        }
    }
}

pub struct RockSource {
    kinds: Vec<RockKind>,
    index: usize,
}
impl RockSource {
    pub fn new() -> Self {
        let kinds = vec![
            RockKind::Horizontal,
            RockKind::Plus,
            RockKind::Corner,
            RockKind::Vertical,
            RockKind::Box,
        ];
        Self { index: 0, kinds }
    }
    pub fn next(&mut self) -> &RockKind {
        let result = self.kinds.get(self.index);
        self.index = (self.index + 1) % self.kinds.len();
        result.unwrap()
    }
    pub fn current_index(&self) -> usize {
        self.index
    }
    pub fn advance_index(&mut self, steps: u64) {
        self.index = (self.index + steps as usize) % self.kinds.len();
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn perpetual_rock_source() {
        let mut input = RockSource::new();
        let result = input.next();
        assert_eq!(result, &RockKind::Horizontal);
        let result = input.next();
        assert_eq!(result, &RockKind::Plus);
        let result = input.next();
        assert_eq!(result, &RockKind::Corner);
        let result = input.next();
        assert_eq!(result, &RockKind::Vertical);
        let result = input.next();
        assert_eq!(result, &RockKind::Box);
        let result = input.next();
        assert_eq!(result, &RockKind::Horizontal);
    }

    #[test]
    fn basic_shape_operations() {
        let mut source = RockSource::new();
        let anchor = Point { x: 1, y: 3 };
        let shape = source.next().to_shape(&anchor);
        assert_eq!(shape.anchor, anchor);

        let current_points = shape.get_current_points();
        let expected_current_points = vec![
            Point { x: 1, y: 3 },
            Point { x: 2, y: 3 },
            Point { x: 3, y: 3 },
            Point { x: 4, y: 3 },
        ];
        assert_eq!(current_points, expected_current_points);
        let map = map::Map::new();

        let mut current_shape = ShapeState::Falling(shape);
        current_shape.move_shape(ShapeMove::Sideways(Side::Right), &map);
        if let ShapeState::Falling(falling_shape) = &current_shape {
            let current_points = falling_shape.get_current_points();
            let expected_current_points = vec![
                Point { x: 2, y: 3 },
                Point { x: 3, y: 3 },
                Point { x: 4, y: 3 },
                Point { x: 5, y: 3 },
            ];
            assert_eq!(current_points, expected_current_points);
        } else {
            panic!("Expected ShapeState::Falling");
        }

        current_shape.move_shape(ShapeMove::Sideways(Side::Left), &map);
        if let ShapeState::Falling(falling_shape) = &current_shape {
            let current_points = falling_shape.get_current_points();
            let expected_current_points = vec![
                Point { x: 1, y: 3 },
                Point { x: 2, y: 3 },
                Point { x: 3, y: 3 },
                Point { x: 4, y: 3 },
            ];
            assert_eq!(current_points, expected_current_points);
        } else {
            panic!("Expected ShapeState::Falling");
        }

        current_shape.move_shape(ShapeMove::Sideways(Side::Left), &map);
        if let ShapeState::Falling(falling_shape) = &current_shape {
            let current_points = falling_shape.get_current_points();
            let expected_current_points = vec![
                Point { x: 1, y: 3 },
                Point { x: 2, y: 3 },
                Point { x: 3, y: 3 },
                Point { x: 4, y: 3 },
            ];
            assert_eq!(current_points, expected_current_points);
        } else {
            panic!("Expected ShapeState::Falling");
        }

        current_shape.move_shape(ShapeMove::Down, &map);
        if let ShapeState::Falling(falling_shape) = &current_shape {
            let current_points = falling_shape.get_current_points();
            let expected_current_points = vec![
                Point { x: 1, y: 2 },
                Point { x: 2, y: 2 },
                Point { x: 3, y: 2 },
                Point { x: 4, y: 2 },
            ];
            assert_eq!(current_points, expected_current_points);
        } else {
            panic!("Expected ShapeState::Falling");
        }
    }
}
