use crate::day17::rocks;
use std::collections::HashMap;

const MAX_MAP_HEIGHT: u32 = 50;
const LEFT_WALL_X: u32 = 0;
const RIGHT_WALL_X: u32 = 8;
const FLOOR_Y: u32 = 0;
const MAP_Y_GROWTH: u32 = 4;
const MAP_X_SHAPE_SPAWN: u32 = 3;

pub struct Map {
    occupied_spaces: HashMap<rocks::Point, bool>,
}
impl Map {
    pub fn new() -> Self {
        let mut new_self = Self {
            occupied_spaces: HashMap::new(),
        };
        new_self.build_floor();
        new_self.grow_map_accordingly();
        new_self
    }
    pub fn add_settled_shape(&mut self, shape: rocks::SettledShape) {
        for point in shape.points {
            self.occupied_spaces.insert(point, true);
        }
        self.grow_map_accordingly();
    }
    pub fn get_current_height(&self) -> u32 {
        self.occupied_spaces
            .iter()
            .filter(|(p, occupied)| p.x != LEFT_WALL_X && p.x != RIGHT_WALL_X && **occupied)
            .map(|(p, _)| p.y)
            .max()
            .unwrap()
    }
    fn build_floor(&mut self) {
        for x in LEFT_WALL_X..=RIGHT_WALL_X {
            self.occupied_spaces
                .insert(rocks::Point { x, y: FLOOR_Y }, true);
        }
    }
    fn grow_map_accordingly(&mut self) {
        let current_height = self.get_current_height();
        for y in (current_height + 1)..=(current_height + MAP_Y_GROWTH) {
            for x in LEFT_WALL_X..=RIGHT_WALL_X {
                let occupied = matches!(x, LEFT_WALL_X | RIGHT_WALL_X);
                self.occupied_spaces.insert(rocks::Point { x, y }, occupied);
            }
        }
        self.retain_top_of_the_map();
    }
    fn retain_top_of_the_map(&mut self) {
        let current_height = self.get_current_height();
        if current_height > MAX_MAP_HEIGHT {
            self.occupied_spaces
                .retain(|point, _| point.y > (current_height - MAX_MAP_HEIGHT));
        }
    }
    pub fn are_points_free(&self, shape: &rocks::Shape) -> bool {
        let points = shape.get_current_points();
        points.iter().all(|point| {
            let occupied_space = self.occupied_spaces.get(point);
            occupied_space == Some(&false) || occupied_space.is_none()
        })
    }
    pub fn get_new_shape_position(&self) -> rocks::Point {
        rocks::Point {
            x: MAP_X_SHAPE_SPAWN,
            y: self.get_current_height() + MAP_Y_GROWTH,
        }
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn map_initialize() {
        let result = Map::new();
        assert_eq!(result.occupied_spaces.len(), 5 * 9);
        assert_eq!(
            result.occupied_spaces.get(&rocks::Point { x: 0, y: 0 }),
            Some(&true)
        );
        assert_eq!(
            result.occupied_spaces.get(&rocks::Point { x: 5, y: 0 }),
            Some(&true)
        );
        assert_eq!(
            result.occupied_spaces.get(&rocks::Point { x: 1, y: 1 }),
            Some(&false)
        );
        assert_eq!(
            result.occupied_spaces.get(&rocks::Point { x: 8, y: 1 }),
            Some(&true)
        );
        assert_eq!(
            result.occupied_spaces.get(&rocks::Point { x: 4, y: 3 }),
            Some(&false)
        );
    }

    #[test]
    fn map_settling() {
        let settled = rocks::RockKind::Vertical
            .to_shape(&rocks::Point { x: 3, y: 1 })
            .to_settled();
        let mut result = Map::new();
        result.add_settled_shape(settled);
        assert_eq!(
            result.occupied_spaces.get(&rocks::Point { x: 0, y: 0 }),
            Some(&true)
        );
        assert_eq!(
            result.occupied_spaces.get(&rocks::Point { x: 3, y: 1 }),
            Some(&true)
        );
        assert_eq!(
            result.occupied_spaces.get(&rocks::Point { x: 3, y: 4 }),
            Some(&true)
        );
        assert_eq!(
            result.occupied_spaces.get(&rocks::Point { x: 2, y: 4 }),
            Some(&false)
        );
        assert_eq!(
            result.occupied_spaces.get(&rocks::Point { x: 3, y: 5 }),
            Some(&false)
        );
    }

    #[test]
    fn map_growing() {
        let settled = rocks::RockKind::Vertical
            .to_shape(&rocks::Point { x: 3, y: 1 })
            .to_settled();
        let mut result = Map::new();
        result.add_settled_shape(settled);
        result.grow_map_accordingly();
        assert_eq!(
            result.occupied_spaces.get(&rocks::Point { x: 3, y: 5 }),
            Some(&false)
        );
        assert_eq!(
            result.occupied_spaces.get(&rocks::Point { x: 3, y: 8 }),
            Some(&false)
        );
    }

    #[test]
    fn map_free() {
        let settled = rocks::RockKind::Vertical
            .to_shape(&rocks::Point { x: 3, y: 1 })
            .to_settled();
        let mut map = Map::new();
        map.add_settled_shape(settled);
        map.grow_map_accordingly();
        let new_shape = rocks::RockKind::Horizontal.to_shape(&rocks::Point { x: 3, y: 2 });
        let result = map.are_points_free(&new_shape);
        assert_eq!(result, false);
    }
}
