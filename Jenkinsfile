pipeline {
    agent any

    tools {
        nodejs 'NodeJs20' // Name of NodeJS installation in Jenkins
    }

    environment {
        DOCKER_IMAGE_CLIENT = 'aroshsprodhen/linkup-client:latest'
        DOCKER_IMAGE_API    = 'aroshsprodhen/linkup-api:latest'
    }

    triggers {
        // Trigger build on GitHub push
        githubPush()
    }

    stages {

        stage('Checkout Code') {
            steps {
                git branch: 'Jenkins', 
                    credentialsId: 'jenkins-ci-cd-token', 
                    url: 'https://github.com/Prodhen/LinkUp.git'
            }
        }

        stage('Build API') {
            steps {
                dir('API') {
                    bat 'dotnet restore'
                    bat 'dotnet build --configuration Release --no-restore'
                    bat 'dotnet publish -c Release -o publish'
                }
            }
        }

        stage('Build Client') {
            steps {
                dir('client') {
                    bat 'npm install'
                    bat 'npm run build --prod'
                }
            }
        }

        stage('Build & Push Docker Images') {
            steps {
                withCredentials([usernamePassword(credentialsId: 'docker-hub-credentials-id', usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASS')]) {
                        script {
                            // Login to Docker Hub
                            sh "echo $DOCKER_PASS | docker login -u $DOCKER_USER --password-stdin"

                            // Build and push client
                            sh "docker build -t aroshprodhen/linkup-client:latest ./client"
                            sh "docker push aroshprodhen/linkup-client:latest"

                            // Build and push API
                            sh "docker build -t aroshprodhen/linkup-api:latest ./API/publish"
                            sh "docker push aroshprodhen/linkup-api:latest"
                        }
                    }
            }
        }

        stage('Deploy with Docker-Compose') {
            steps {
                dir("${WORKSPACE}") {
                    bat """
                    docker pull %DOCKER_IMAGE_CLIENT%
                    docker pull %DOCKER_IMAGE_API%
                    
                    docker-compose down
                    docker-compose up -d --build
                    """
                }
            }
        }
    }

    post {
        always {
            echo 'Pipeline finished!'
        }
        success {
            echo 'Deployment successful!'
        }
        failure {
            echo 'Pipeline failed!'
        }
    }
}
