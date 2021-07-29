import Answer from './Answer';

export default interface Round {
    id: number,
    prompt: string,
    answers: Answer[],
    active: boolean
}
