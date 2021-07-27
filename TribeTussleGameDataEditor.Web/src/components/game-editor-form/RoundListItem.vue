<template>
    <li :class="{ 'list-group-item': true, 'bg-secondary': activeHover && !active, 'bg-primary': active,
        'text-white': activeHover || active }"
        @mouseover="toggleHover()" @mouseout="toggleHover()" @click="setActiveRound()">
        <div>
            <b-row>
                <b-col v-if="isMobile">
                    {{ isFastMoney ? 'FM' : index }}
                </b-col>
                <b-col v-else>
                    {{ isFastMoney ? 'Fast Money' : index }}: {{round.prompt}}
                </b-col>
                <b-col class="text-right col-sm-auto">
                    <div class="drag-handle" v-if="!isFastMoney">
                        <b-icon-grip-horizontal></b-icon-grip-horizontal>
                    </div>
                </b-col>
            </b-row>
        </div>
    </li>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from 'vue-property-decorator';
    import Round from '@/components/types/Round';

    @Component
    export default class RoundList extends Vue {
        @Prop() readonly round!: Round;
        @Prop() readonly index!: number;
        @Prop() readonly active!: boolean;
        @Prop() readonly isFastMoney!: boolean;
        @Prop() readonly setActive!: (roundId: number) => void;

        private activeHover: boolean = false;

        public toggleHover(): void {
            this.activeHover = !this.activeHover;
        }

        public setActiveRound() {
            this.setActive(this.round.id);
        }
    }
</script>

<style scoped>
    .list-group-item:hover {
        cursor: pointer;
    }

    .drag-handle:hover {
        cursor: move;
    }
</style>