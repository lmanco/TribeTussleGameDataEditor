<template>
    <div>
        <b-form inline>
            <b-container class="mt-4 ml-4 mr-4 bg-light border rounded-lg" fluid>
                <b-row class="mt-3">
                    <b-col>
                        <b-form-group id="form-group-game-data-name"
                                      label="Name:"
                                      label-size="lg"
                                      label-cols="1"
                                      label-for="round-form-input-prompt">
                            <b-form-input id="form-input-game-data-name"
                                          v-model="gameDataName"
                                          required
                                          size="lg"
                                          class="w-100"
                                          placeholder="Enter game data name">
                            </b-form-input>
                        </b-form-group>
                    </b-col>
                </b-row>
                <b-row class="mt-3 mb-3 pl-2">
                    <b-col cols="4" class="pl-2 pr-1">
                        <round-list :initRounds="rounds" />
                    </b-col>
                    <b-col cols="8" class="pl-2">
                        <div class="border">
                            <div class="m-3">
                                <h5>Round 1</h5>
                                <b-form-group id="form-group-prompt"
                                              label="Prompt:"
                                              label-cols="1"
                                              class="ml-1"
                                              label-for="form-input-prompt">
                                    <b-form-input id="form-input-prompt"
                                                  v-model="prompt"
                                                  required
                                                  class="w-100"
                                                  placeholder="Enter prompt">
                                    </b-form-input>
                                </b-form-group>
                            </div>
                        </div>
                    </b-col>
                </b-row>
            </b-container>
        </b-form>
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from 'vue-property-decorator';
    import RoundList from './RoundList.vue';
    import Round from '@/components/types/Round';

    @Component({
        components: {
            RoundList
        }
    })
    export default class GameEditorForm extends Vue {
        @Prop() readonly initGameDataName!: string;
        @Prop() readonly rounds!: Round[];

        private gameDataName: string = '';
        private prompt: string = '';

        public mounted(): void {
            if (this.initGameDataName)
                this.gameDataName = this.initGameDataName;
        }
    }
</script>

<style scoped>
</style>