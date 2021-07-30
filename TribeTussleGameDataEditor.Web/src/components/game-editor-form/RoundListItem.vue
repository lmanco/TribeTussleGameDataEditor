<template>
    <li :class="{ 'list-group-item': true,
        'bg-secondary': forFastMoneyQuestionList ? !activeHover && !active : activeHover && !active,
        'bg-primary': forFastMoneyQuestionList ? activeHover && !active : active,
        'bg-light': (forFastMoneyQuestionList && active) || (!forFastMoneyQuestionList && !active && !activeHover),
        'text-white': forFastMoneyQuestionList ? !active || (activeHover && !active) : (activeHover || active),
        'for-fm': forFastMoneyQuestionList,
        'fm-active-tab': forFastMoneyQuestionList && active,
        'item-top-border': forFastMoneyQuestionList || index === 0, 'fm-first': forFastMoneyQuestionList && index === 0 }"
        :id="`round-list-item-${round.id}`"
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
                <b-col :class="{ 'text-truncate': true, 'ml-3': isFastMoney }"
                       :style="forFastMoneyQuestionList && active ? 'cursor: default;' : ''">
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
        @Prop() readonly forFastMoneyQuestionList!: boolean;

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
    .list-group-item:hover:not(.fm-active-tab) {
        cursor: pointer;
    }

    .drag-handle:hover {
        cursor: move;
    }

    .list-group-item:not(.item-top-border) {
        border-top: none;
    }

    .fm-first {
        border-left: none;
    }

    .for-fm {
        border-right: none;
        border-bottom: none;
        border-bottom-left-radius: unset !important;
        border-bottom-right-radius: unset !important;
    }

    .bg-background-light {
        background-color: #808080;
    }
</style>