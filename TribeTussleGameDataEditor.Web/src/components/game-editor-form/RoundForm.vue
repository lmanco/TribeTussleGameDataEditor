<template>
    <div class="border">
        <div :class="{ 'm-3': true, 'text-center': isMobile }">
            <h5>{{ roundTitle }}</h5>
            <b-form-group id="form-group-prompt"
                          :label="isMobile ? '' : 'Prompt:'"
                          :label-cols="isMobile ? 0 : (isTablet ? 3 : 1)"
                          :class="{ 'ml-1': mqOrdinal > mqOrdinals.tablet }"
                          label-for="form-input-prompt">
                <b-form-input id="form-input-prompt"
                              v-model="round.prompt"
                              required
                              class="w-100"
                              placeholder="Enter prompt">
                </b-form-input>
            </b-form-group>
            <b-button variant="danger" v-if="!isFastMoneyRound">Delete Round</b-button>
        </div>
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from 'vue-property-decorator';
    import draggable from 'vuedraggable';
    import Round from '@/components/types/Round';

    @Component({
        components: {
            draggable
        }
    })
    export default class RoundList extends Vue {
        @Prop() readonly round!: Round;
        @Prop() readonly roundTitle!: string;

        get isFastMoneyRound() {
            return this.roundTitle === 'Fast Money Round';
        }
    }
</script>

<style scoped>
</style>