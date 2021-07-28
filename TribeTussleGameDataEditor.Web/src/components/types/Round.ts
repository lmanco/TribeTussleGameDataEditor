import Answer from './Answer.vue';

export default interface Round {
    id: number,
    prompt: string,
    answers: Answer[],
    active: boolean
}
