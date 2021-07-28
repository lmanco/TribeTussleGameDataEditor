<template>
    <li :class="{ 'list-group-item': true, 'bg-secondary': activeHover && !active, 'bg-primary': active,
        'text-white': activeHover || active }" :id="`round-list-item-${round.id}`"
        @mouseover="toggleHover()" @mouseout="toggleHover()" @click="setActiveRound()">
        <div>
            <b-tooltip :target="`round-list-item-${round.id}`" triggers="hover" :delay="{ show: 1000 }" v-if="mqOrdinal >= mqOrdinals.laptop && round.prompt">
                {{ round.prompt }}
            </b-tooltip>
            <b-row>
                <b-col v-if="isMobile">
                    {{ isFastMoney ? 'FM' : index + 1 }}
                </b-col>
                <b-col v-else class="text-truncate">
                    {{ isFastMoney ? 'Fast Money' : index + 1 }}{{round.prompt ? ':' : '' }} {{round.prompt}}
                </b-col>
                <b-col :class="{ 'text-center': mqOrdinal <= mqOrdinals.tablet, 'text-right': mqOrdinal >= mqOrdinals.laptop,
                       'col-sm-auto': true }">
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
        @Prop() readonly setActive!: (roundId: number, index: number) => void;

        private activeHover: boolean = false;

        public toggleHover(): void {
            this.activeHover = !this.activeHover;
        }

        public setActiveRound() {
            this.setActive(this.round.id, this.index);
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