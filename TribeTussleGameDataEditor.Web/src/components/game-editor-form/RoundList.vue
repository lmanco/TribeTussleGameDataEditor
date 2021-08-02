<template>
    <div class="isFastMoneyQuestionList ? 'border-top' : 'border'">
        <div :class="{ 'm-3': !isFastMoneyQuestionList, 'mb-3': isFastMoneyQuestionList, 'text-center': isMobile }">
            <h5 v-if="!isFastMoneyQuestionList" style="cursor: default;">
                Round
            </h5>
            <h6 v-else style="cursor: default;" class="ml-3 mr-3 mt-2">
                Question:
            </h6>
            <b-button v-if="!isFastMoneyQuestionList" class="mb-2 w-100" variant="primary" @click="addRound">
                <b-icon-plus></b-icon-plus>{{ isMobile ? '' : 'Add Round' }}
            </b-button>
            <draggable v-if="!isFastMoneyQuestionList" v-model="rounds" handle=".drag-handle" :move="roundMoved" @change="roundDropped">
                <transition-group>
                    <ul :class="{ 'list-group': true, 'first-round-list-item': index === 0,
                        'last-round-list-item': index === rounds.length -1 }"
                        v-for="(round, index) in rounds" :key="round.id">
                        <round-list-item 
                                         :round="round" :index="index" :active="round.id === activeRoundId" 
                                         :isFastMoney="index === rounds.length - 1"
                                         :setActive="setActiveRound" />
                    </ul>
                </transition-group>
            </draggable>
            <draggable v-else class="row ml-0 mr-0" v-model="rounds" handle=".drag-handle" :move="roundMoved" @change="roundDropped">
                <b-col :class="{ 'list-group': true, 'first-round-list-item-fm': index === 0,
                       'last-round-list-item-fm': index === rounds.length -1,
                       'mr-0': true, 'pl-0': true, 'pr-0': true, 'w-25': isMobile }"
                       v-for="(round, index) in rounds" :key="round.id">
                    <round-list-item :round="round" :index="index" :active="round.id === activeRoundId" 
                                     :forFastMoneyQuestionList="true"
                                     :setActive="setActiveRound" />
                </b-col>
            </draggable>
        </div>
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from 'vue-property-decorator';
    import draggable from 'vuedraggable';
    import Round from '@/components/types/Round';
    import RoundListItem from './RoundListItem.vue';

    @Component({
        components: {
            draggable,
            RoundListItem
        }
    })
    export default class RoundList extends Vue {
        @Prop() readonly initRounds!: Round[];
        @Prop() readonly updateActiveRound!: (round: Round, title: string) => void;
        @Prop() readonly isFastMoneyQuestionList!: boolean;
        @Prop() readonly initSelectedId!: number;
        @Prop() readonly updateFullList!: (rounds: Round[]) => void;

        private rounds: Round[] = [];
        private activeRoundId: number = 1;
        private activeRoundIndex: number = 0;

        public mounted(): void {
            if (this.initRounds)
                this.rounds = this.initRounds;
            if (this.initSelectedId) {
                this.activeRoundId = this.initSelectedId;
                this.activeRoundIndex = this.rounds.findIndex(round => round.id === this.activeRoundId);
            }
        }

        public setActiveRound(roundId: number, index: number): void {
            this.activeRoundId = roundId;
            this.activeRoundIndex = index;
            this.updateActiveRound(this.rounds[index], this.getRoundTitle(index));
        }

        public roundMoved(event: any): boolean {
            return this.isFastMoneyQuestionList || event.draggedContext.futureIndex < this.rounds.length - 1;
        }

        public roundDropped(event: any): void {
            if (event.moved.element.id === this.activeRoundId) {
                const index: number = event.moved.newIndex;
                this.updateActiveRound(this.rounds[index], this.getRoundTitle(index));
            }
            else {
                const currentRoundIndex: number = this.rounds.findIndex((round: Round) => round.id === this.activeRoundId);
                if (currentRoundIndex > -1 && currentRoundIndex !== this.activeRoundIndex) {
                    this.activeRoundIndex = currentRoundIndex;
                    this.updateActiveRound(this.rounds[this.activeRoundIndex], this.getRoundTitle(this.activeRoundIndex));
                }
            }
            if (event.moved.newIndex !== event.moved.oldIndex)
                this.$emit('roundChanged');
            if (typeof this.updateFullList === 'function')
                this.updateFullList(this.rounds);
        }

        public addRound(): void {
            const nextRoundId: number = Math.max(...this.rounds.map((round: Round) => round.id)) + 1;
            this.rounds = [
                ...this.rounds.slice(0, this.rounds.length - 1),
                { id: nextRoundId, prompt: '', scale: 1, answers: [{ id: 1, text: '', value: 0 }], active: true },
                this.rounds[this.rounds.length - 1]
            ];
            this.activeRoundIndex = this.rounds.length - 2;
            this.setActiveRound(nextRoundId, this.activeRoundIndex);
            this.$emit('numRoundsChanged', this.rounds.length);
            this.$emit('roundChanged');
        }

        public deleteRound(roundId: number): void {
            this.rounds = this.rounds.filter(round => round.id != roundId);
            if (this.activeRoundIndex > this.rounds.length - 2)
                this.activeRoundIndex--;
            this.setActiveRound(this.rounds[this.activeRoundIndex].id, this.activeRoundIndex);
            this.$emit('numRoundsChanged', this.rounds.length);
            this.$emit('roundChanged');
        }

        private getRoundTitle(index: number): string {
            return index < this.rounds.length - 1 ? `Round ${index + 1}` : 'Fast Money Round';
        }
    }
</script>

<style scoped>
    ul:not(.first-round-list-item) {
        border-top-left-radius: unset;
        border-top-right-radius: unset;
        border-top: none;
    }

    ul:not(.last-round-list-item) {
        border-bottom-left-radius: unset;
        border-bottom-right-radius: unset;
        border-bottom: none;
    }
</style>