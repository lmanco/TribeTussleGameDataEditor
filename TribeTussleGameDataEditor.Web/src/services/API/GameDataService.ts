import { Requester, RoutePrefix } from '@/services/API/Requester';
import GameData from '@/services/API/types/GameData';

export interface GameDataService {
    getGameNames(): Promise<string[]>;
    getGameData(gameName: string): Promise<GameData>;
    exportGameData(gameName: string): void;
    createGameData(gameData: GameData): Promise<void>;
    updateGameData(gameName: string, gameData: GameData): Promise<void>;
    deleteGameData(gameName: string): Promise<void>;
}

class Constants {
    public static readonly GAME_DATA_PATH: string = 'game-data';
    public static readonly GAME_DATA_FILE_PATH: string = 'game-data-file';
}

export default class RequesterGameDataService implements GameDataService {
    private requester: Requester;
    private dataRootUrl: string;

    public constructor(requester: Requester, dataRootUrl: string) {
        this.requester = requester;
        this.dataRootUrl = dataRootUrl;
    }

    public async getGameNames(): Promise<string[]> {
        return (await this.requester.get(Constants.GAME_DATA_PATH)).data;
    }

    public async getGameData(gameName: string): Promise<GameData> {
        return (await this.requester.get(this.getGameDataNamePath(gameName))).data;
    }

    public exportGameData(gameName: string): void {
        window.location.href = `${this.dataRootUrl}${RoutePrefix}${Constants.GAME_DATA_FILE_PATH}/${gameName}`;
    }

    public async createGameData(gameData: GameData): Promise<void> {
        await this.requester.post(Constants.GAME_DATA_PATH, gameData);
    }

    public async updateGameData(gameName: string, gameData: GameData): Promise<void> {
        await this.requester.put(this.getGameDataNamePath(gameName), gameData);
    }

    public async deleteGameData(gameName: string): Promise<void> {
        await this.requester.delete(this.getGameDataNamePath(gameName));
    }

    private getGameDataNamePath(name: string): string {
        return `${Constants.GAME_DATA_PATH}/${name}`;
    }
}
