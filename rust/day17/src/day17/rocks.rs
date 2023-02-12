const LEFT_WALL_X: u32 = 0;
const RIGHT_WALL_X: u32 = 8;
const FLOOR_Y: u32 = 0;

#[derive(Debug, PartialEq, Clone)]
struct Point {
    x: u32,
    y: u32,
}

enum Side {
    Left,
    Right,
}
enum ShapeMove {
    Sideways(Side),
    Down,
}

#[derive(Debug, PartialEq, Clone)]
struct SettledShape {
    points: Vec<Point>,
}

#[derive(Debug, PartialEq, Clone)]
enum ShapeState {
    Settled(SettledShape),
    Falling(Shape),
}
impl ShapeState {
    fn move_shape(self, movement: ShapeMove) -> ShapeState {
        match self {
            ShapeState::Settled(_) => self,
            ShapeState::Falling(shape) => {
                let mut new_shape = shape.clone();
                match movement {
                    ShapeMove::Sideways(side) => {
                        match side {
                            Side::Left => {
                                new_shape.anchor.x -= 1;
                                if new_shape.anchor.x == LEFT_WALL_X {
                                    return ShapeState::Falling(shape);
                                }
                            }
                            Side::Right => {
                                new_shape.anchor.x += 1;
                                if new_shape.anchor.x >= RIGHT_WALL_X {
                                    return ShapeState::Falling(shape);
                                }
                            }
                        }
                        ShapeState::Falling(new_shape)
                    }
                    ShapeMove::Down => {
                        new_shape.anchor.y -= 1;
                        if new_shape.anchor.y == FLOOR_Y {
                            return ShapeState::Settled(shape.to_settled());
                        }
                        ShapeState::Falling(new_shape)
                    }
                }
            }
        }
    }
}

#[derive(Debug, PartialEq, Clone)]
struct Shape {
    points: Vec<Point>,
    anchor: Point,
}
impl Shape {
    fn to_settled(&self) -> SettledShape {
        SettledShape {
            points: self.get_current_points(),
        }
    }
    fn get_current_points(&self) -> Vec<Point> {
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
enum RockKind {
    Horizontal,
    Plus,
    Corner,
    Vertical,
    Box,
}
impl RockKind {
    fn to_shape(&self, anchor: &Point) -> Option<Shape> {
        if anchor.x == LEFT_WALL_X || anchor.x >= RIGHT_WALL_X {
            return None;
        }

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
        Some(Shape {
            points,
            anchor: anchor.clone(),
        })
    }
}

struct RockSource {
    kinds: Vec<RockKind>,
    index: usize,
}
impl RockSource {
    fn new() -> Self {
        let kinds = vec![
            RockKind::Horizontal,
            RockKind::Plus,
            RockKind::Corner,
            RockKind::Vertical,
            RockKind::Box,
        ];
        Self { index: 0, kinds }
    }
    fn next(&mut self) -> &RockKind {
        let result = self.kinds.get(self.index);
        self.index = (self.index + 1) % self.kinds.len();
        result.unwrap()
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
        let shape = source.next().to_shape(&anchor).unwrap();
        assert_eq!(shape.anchor, anchor);

        let current_points = shape.get_current_points();
        let expected_current_points = vec![
            Point { x: 1, y: 3 },
            Point { x: 2, y: 3 },
            Point { x: 3, y: 3 },
            Point { x: 4, y: 3 },
        ];
        assert_eq!(current_points, expected_current_points);

        let mut current_shape = ShapeState::Falling(shape);
        current_shape = current_shape.move_shape(ShapeMove::Sideways(Side::Right));
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

        current_shape = current_shape.move_shape(ShapeMove::Sideways(Side::Left));
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

        current_shape = current_shape.move_shape(ShapeMove::Sideways(Side::Left));
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

        current_shape = current_shape.move_shape(ShapeMove::Down);
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
