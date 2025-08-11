import { Photo } from "./Photo"

export interface Member {
    id: number
    userName: string
    age: number
    photoUrl: string
    knownAs: string
    introduction: string
    interests: string
    lookingFor: string
    created: Date
    lastActive: Date
    gender: string
    city: string
    country: string
    photos: Photo[]
}