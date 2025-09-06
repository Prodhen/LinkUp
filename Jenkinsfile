pipeline {
    agent any 
       tools {
        nodejs 'NodeJs20' // Use the name you configured
    }

stages {
        stage('Checkout Code') {
            steps {

                git branch: 'main', credentialsId: 'github-pat-for-jenkins', url: 'https://github.com/Prodhen/LinkUp.git'
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

        stage('API Build & Test') {
            steps {
                dir('API') { 
                    sh 'dotnet restore'
                    sh 'dotnet build --configuration Release --no-restore'
         
                }
            }
        }

        stage('Build & Push Docker Images') {
            steps {
                script {
 
                    withDockerRegistry(credentialsId: 'docker-hub-credentials-id', url: 'https://index.docker.io/v1/') {
       
                        sh "docker build -t prodhen/linkup-client:latest ./client"
                        sh "docker push prodhen/linkup-client:latest"

        
                        sh "docker build -t prodhen/linkup-api:latest ./API"
                        sh "docker push prodhen/linkup-api:latest"
                    }
                }
            }
        }

        stage('Deploy on Local PC') {
                    steps {
                        script {
                
                            def projectPath = "/cygdrive/d/LinkUp-V1/LinkUp" 
                            sh """
                                # Pull latest images
                                docker pull prodhen/linkup-client:latest
                                docker pull prodhen/linkup-api:latest

                                # Change directory to where docker-compose.yml is located
                                cd ${projectPath}

                                # Stop and remove existing containers, then start new ones
                                docker-compose down
                                docker-compose up -d
                            """
                        }
                    }
        }
            
}
} 
        