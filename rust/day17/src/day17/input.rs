#[derive(Debug, PartialEq)]
enum AirJetBlowingDirection {
    Left,
    Right,
}

struct AirJets {
    index: usize,
    air_jets: Vec<AirJetBlowingDirection>,
}
impl AirJets {
    fn new(air_jets: Vec<AirJetBlowingDirection>) -> Self {
        Self { index: 0, air_jets }
    }
    fn next(&mut self) -> &AirJetBlowingDirection {
        let result = self.air_jets.get(self.index);
        self.index = (self.index + 1) % self.air_jets.len();
        result.unwrap()
    }
}

fn read_input(input: &str) -> AirJets {
    let directions = input
        .chars()
        .map(|c| match c {
            '<' => AirJetBlowingDirection::Left,
            '>' => AirJetBlowingDirection::Right,
            _ => panic!("Invalid input"),
        })
        .collect();
    AirJets::new(directions)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_read_input() {
        let input = "<>><<";
        let expected = vec![
            AirJetBlowingDirection::Left,
            AirJetBlowingDirection::Right,
            AirJetBlowingDirection::Right,
            AirJetBlowingDirection::Left,
            AirJetBlowingDirection::Left,
        ];
        let result = read_input(input);
        assert_eq!(result.air_jets, expected);
    }

    #[test]
    fn air_jets_perpetual_direction() {
        let mut input = AirJets::new(vec![
            AirJetBlowingDirection::Left,
            AirJetBlowingDirection::Right,
            AirJetBlowingDirection::Right,
        ]);
        let result = input.next();
        assert_eq!(result, &AirJetBlowingDirection::Left);
        let result = input.next();
        assert_eq!(result, &AirJetBlowingDirection::Right);
        let result = input.next();
        assert_eq!(result, &AirJetBlowingDirection::Right);
        let result = input.next();
        assert_eq!(result, &AirJetBlowingDirection::Left);
        let result = input.next();
        assert_eq!(result, &AirJetBlowingDirection::Right);
    }
}