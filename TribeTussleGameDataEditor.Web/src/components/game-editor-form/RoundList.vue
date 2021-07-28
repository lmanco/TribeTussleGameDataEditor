<template>
    <div class="border">
        <div :class="{ 'm-3': true, 'text-center': isMobile }">
            <h5>Round</h5>
            <b-button class="mb-2 w-100" variant="primary" @click="addRound">
                <b-icon-plus></b-icon-plus>{{ isMobile ? '' : 'Add Round' }}
            </b-button>
            <draggable v-model="rounds" handle=".drag-handle" :move="roundMoved" @change="roundDropped">
                <transition-group>
                    <ul class="list-group" v-for="(round, index) in rounds" :key="round.id">
                        <round-list-item :round="round" :index="index" :active="round.id === activeRoundId" 
                                         :isFastMoney="index === rounds.length - 1" :setActive="setActiveRound" />
                    </ul>
                </transition-group>
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

        private rounds: Round[] = [];
        private activeRoundId: number = 1;
        private activeRoundIndex: number = 0;

        public mounted(): void {
            if (this.initRounds)
                this.rounds = this.initRounds;
        }

        public setActiveRound(roundId: number, index: number): void {
            this.activeRoundId = roundId;
            this.activeRoundIndex = index;
            this.updateActiveRound(this.rounds[index], this.getRoundTitle(index));
        }

        public roundMoved(event: any): boolean {
            return event.draggedContext.futureIndex < this.rounds.length - 1;
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
        }

        public addRound(): void {
            const nextRoundId: number = Math.max(...this.rounds.map((round: Round) => round.id)) + 1;
            this.rounds = [
                ...this.rounds.slice(0, this.rounds.length - 1),
                { id: nextRoundId, prompt: '', answers: [], active: true },
                this.rounds[this.rounds.length - 1]
            ];
            this.activeRoundIndex = this.rounds.length - 2;
            this.setActiveRound(nextRoundId, this.activeRoundIndex);
        }

        private getRoundTitle(index: number): string {
            return index < this.rounds.length - 1 ? `Round ${index + 1}` : 'Fast Money Round';
        }
    }
</script>

<style scoped>
</style>