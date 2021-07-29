export default interface GameData {
    name: string,
    data: GameDataData
}

export interface GameDataData {
    questions: MainGameQuestion[],
    fastMoney: Question[]
}

export interface Question {
    prompt: string,
    answers: Answer[]
}

export interface MainGameQuestion extends Question {
    scale: number
}

export interface Answer {
    text: string,
    value: number
}
