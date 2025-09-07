pipeline {
    agent any 
       tools {
        nodejs 'NodeJs20' // Use the name you configured
    }

stages {
        stage('Checkout Code') {
            steps {

                git branch: 'Jenkins', credentialsId: 'github-pat-for-jenkins', url: 'https://github.com/Prodhen/LinkUp.git'
            }
        }
        
        stage('API Build & Test') {
            agent { // This 'agent' block is necessary
                docker {
                    image 'mcr.microsoft.com/dotnet/sdk:8.0' // Use the .NET 8 SDK image
                    args '-u root' // Often necessary for permissions inside the container
                }
            }
            steps {
                dir('API') { 
                    sh 'dotnet restore'
                    sh 'dotnet build --configuration Release --no-restore'
         
                }
            }
        }

        stage('Client Build & Test') {
            steps {
                dir('client') { 
                    sh 'npm install'
                    sh 'npm run build --prod'
    
                }
            }
        }


        stage('Build & Push Docker Images') {
            steps {
                script {
 
                    withDockerRegistry(credentialsId: 'docker-hub-credentials-id', url: 'https://index.docker.io/v1/') {
       
                        sh "docker build -t aroshprodhen/linkup-client:latest ./client"
                        sh "docker push aroshprodhen/linkup-client:latest"

        
                        sh "docker build -t aroshprodhen/linkup-api:latest ./API"
                        sh "docker push aroshprodhen/linkup-api:latest"
                    }
                }
            }
        }

        stage('Deploy on Local PC') {
                    steps {
                         script {
                            sh """
                                # Pull latest images
                                docker pull aroshprodhen/linkup-client:latest
                                docker pull aroshprodhen/linkup-api:latest

                                # Change directory to workspace (where docker-compose.yml exists)
                                cd ${WORKSPACE}

                                # Stop and remove existing containers, then start new ones
                                docker-compose down
                                docker-compose up -d
                            """
                        }
                 
                    }
        }
            
}
} 
        