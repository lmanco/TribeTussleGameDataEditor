<template>
    <li :class="{ 'list-group-item': true, 'bg-secondary': activeHover && !active, 'bg-primary': active,
        'text-white': activeHover || active }" :id="`round-list-item-${round.id}`"
        @mouseover="toggleHover()" @mouseout="toggleHover()" @click="setActiveRound()">
        <div>
            <b-tooltip :target="`round-list-item-${round.id}`" triggers="hover" :delay="{ show: 1000 }" v-if="mqOrdinal > mqOrdinals.tablet && round.prompt">
                {{ round.prompt }}
            </b-tooltip>
            <b-row v-if="!isMobile">
                <b-col class="col-sm-auto pr-0">
                    <div class="drag-handle" v-if="!isFastMoney">
                        <b-icon-grip-horizontal></b-icon-grip-horizontal>
                    </div>
                </b-col>
                <b-col :class="{ 'text-truncate': true, 'ml-3': isFastMoney }">
                    {{ isFastMoney ? 'Fast Money' : index + 1 }}{{round.prompt ? ':' : '' }} {{round.prompt}}
                </b-col>
            </b-row>
            <b-row v-else>
                <b-col v-if="isMobile" class="text-center col-sm-auto">
                    {{ isFastMoney ? 'FM' : index + 1 }}
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