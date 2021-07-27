<template>
    <div class="border">
        <div :class="{ 'm-3': true, 'text-center': isMobile }">
            <h5>Round</h5>
            <b-button class="mb-2" variant="primary">
                <b-icon-plus></b-icon-plus>{{ isMobile ? '' : 'Add Round' }}
            </b-button>
            <draggable v-model="rounds" handle=".drag-handle" :move="roundMoved">
                <transition-group>
                    <ul class="list-group" v-for="(round, index) in rounds" :key="round.id">
                        <round-list-item :round="round" :index="index + 1" :active="round.id === activeRoundId" 
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

        private rounds: Round[] = [];
        private activeRoundId: number = 1;

        public mounted(): void {
            if (this.initRounds)
                this.rounds = this.initRounds;
        }

        public setActiveRound(roundId: number): void {
            this.activeRoundId = roundId;
        }

        public roundMoved(event: any): boolean {
            return event.draggedContext.futureIndex < this.rounds.length - 1;
        }
    }
</script>

<style scoped>
</style>