import Answer from './Answer';

export default interface Round {
    id: number,
    prompt: string,
    scale: number,
    answers: Answer[],
    active: boolean
}
