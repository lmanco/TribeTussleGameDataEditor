<template>
    <li :class="{ 'list-group-item': true, 'item-top-border': index === 0 }">
        <b-row align-v="center" v-if="mqOrdinal >= mqOrdinals.notebook">
            <b-col cols="1" class="col-sm-auto pr-0">
                <span class="drag-handle">
                    <b-icon-grip-horizontal></b-icon-grip-horizontal>
                </span>
                <b-icon-x :id="`delete-answer-button-${index}`" class="ml-3 text-danger"
                          style="cursor: pointer;" @click="deleteAnswer(index)"></b-icon-x>
                <b-tooltip :target="`delete-answer-button-${index}`" triggers="hover" :delay="{ show: 1000 }" v-if="!isMobile">
                    Delete Answer
                </b-tooltip>
            </b-col>
            <b-col cols="1" :style="{ 'max-width': '5%', cursor: 'default' }">
                {{ index + 1 }}.
            </b-col>
            <b-col cols="8">
                <b-form-input id="form-input-text"
                              v-model="answer.text"
                              @input="$emit('roundChanged')"
                              class="w-100"
                              required
                              placeholder="Enter answer text">
                </b-form-input>
            </b-col>
            <b-col cols="1" class="pl-0">
                <b-form-input id="form-input-score"
                              v-model="answer.value"
                              @input="$emit('roundChanged')"
                              type="number"
                              placeholder="0"
                              style="width: 260%;"
                              max="100"
                              required>
                </b-form-input>
            </b-col>
        </b-row>
        <b-row align-v="center" v-else>
            <b-col class="col-sm-auto pr-0" cols="1">
                <span class="drag-handle">
                    <b-icon-grip-horizontal></b-icon-grip-horizontal>
                </span>
                <b-icon-x class="ml-3"></b-icon-x>
                <span class="ml-2" style="cursor: default;">
                    {{ index + 1 }}.
                </span>
            </b-col>
            <b-col cols="7">
                <b-form-input id="form-input-text"
                              v-model="answer.text"
                              @input="$emit('roundChanged')"
                              class="w-100 mb-2"
                              required
                              v-bind="answer.text"
                              @change="updateAnswer(index, answer)"
                              placeholder="Enter text">
                </b-form-input>
                <b-form-input id="form-input-score"
                              v-model="answer.value"
                              @input="$emit('roundChanged')"
                              type="number"
                              placeholder="0"
                              class="w-100"
                              max="100"
                              required>
                </b-form-input>
            </b-col>
        </b-row>
    </li>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from 'vue-property-decorator';
    import Answer from '@/components/types/Answer';

    @Component
    export default class RoundList extends Vue {
        @Prop() readonly answer!: Answer;
        @Prop() readonly index!: number;
        @Prop() readonly deleteAnswer!: (index: number) => void;
        @Prop() readonly updateAnswer!: (index: number, answer: Answer) => void;
    }
</script>

<style scoped>
    .drag-handle:hover {
        cursor: move;
    }
    input[type=number]::-webkit-inner-spin-button,
    input[type=number]::-webkit-outer-spin-button {
        opacity: 1;
    }

    .list-group-item:not(.item-top-border) {
        border-top: none;
    }
</style>